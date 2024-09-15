using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelHandler : MonoBehaviour
{
    private string levelsFolderPath;
    public GameObject gameLogic;
    public GameObject timerObject;
    public GameObject shelfPrefab;
    public Transform parentForShelfTransform;
    public Text currentLevelUI;

    private const int numberOfLevelsMade = 5;


    public static event Action OnFinishLevelLoad;

    //positions for coordonate 0 in the canvas
    //"x": 960.0,
    //"y": 540.0,
    //"z": 0.0


    private void Start()
    {
        levelsFolderPath = Path.Combine(Application.streamingAssetsPath, "Levels");
        LevelProgress levelProgress = LoadProgresFromFile("CurentLevel.json");
        currentLevelUI.text = levelProgress.saveProgress.ToString();
        string levelName = string.Format("LV_{0}.json", levelProgress.saveProgress);
        LevelData levelData = LoadLevelDataFromFile(levelName);
        levelData.InitializeLoadedLevel();
        LoadLevelOnScreen(levelData);
    }

    public void LoadNextLevel()
    {
        LevelProgress levelProgress = LoadProgresFromFile("CurentLevel.json");
        
        if (levelProgress.saveProgress < numberOfLevelsMade)
        {
            levelProgress.saveProgress += 1;
            SaveprogresToFile(levelProgress, "CurentLevel.json");
        }
        else
        {
            //start again form level 1
            levelProgress.saveProgress = 1;
            SaveprogresToFile(levelProgress, "CurentLevel.json");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadLevelOnScreen(LevelData level)
    {
        Timer timer = timerObject.GetComponent<Timer>();
        timer.timeLeft = level.time;
        GameLogic game = gameLogic.GetComponent<GameLogic>();
        game.numberOfItemsLeft = level.totalNumberOfItems;


        for (int i = 0; i < level.shelfPositiosn.Length; i++)
        {
            GameObject newestShelf  = Instantiate(shelfPrefab, level.shelfPositiosn[i], Quaternion.identity, parentForShelfTransform);
            
            ShelfLogic shelfLogic = newestShelf.GetComponent<ShelfLogic>();
            shelfLogic.itemLayerList = level.ReturnItemArrayForShelf(i);
        }
        OnFinishLevelLoad?.Invoke();
    }

    private void SaveLevelToFile(LevelData level, string fileName)
    {
        string json = JsonUtility.ToJson(level, true);
        string filePath = Path.Combine(levelsFolderPath, fileName);
        File.WriteAllText(filePath, json);
        Debug.Log("Level data saved to: " + filePath);
    }

    private LevelData LoadLevelDataFromFile(string fileName)
    {
        string filePath = Path.Combine(levelsFolderPath, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LevelData level = JsonUtility.FromJson<LevelData>(json);
            return level;
        }
        else
        {
            Debug.Log("File does not exist");
            return null;
        }
    }

    private void SaveprogresToFile(LevelProgress levelProgress,string fileName)
    {
        string json = JsonUtility.ToJson(levelProgress, true);
        string filePath = Path.Combine(levelsFolderPath, fileName);
        File.WriteAllText(filePath, json);
    }
    private LevelProgress LoadProgresFromFile(string fileName)
    {
        string filePath = Path.Combine(levelsFolderPath, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LevelProgress levelProgress = JsonUtility.FromJson<LevelProgress>(json);
            return levelProgress;
        }
        else
        {
            Debug.Log("File does not exist");
            return null;
        }
    }
    private class LevelProgress
    {
        public int saveProgress;
    }

    private class LevelData
    {
        public int time;
        public int totalNumberOfItems;          //this is not necesary to be acurate in the savefile because it is calculated in ConvertToMultiDymensionArray() function
        public int rowsForReconstruction;
        public int columnsForReconstruction;
        public Vector3[] shelfPositiosn;        //the location of the shelfs in the level
        public int[] arrayForSerialize;         // i cant serialize 2dimensin array so i convert it in a simple array
        public int[,] items;                    //each row has the items for a shelf (ex row 0 with 0 2 2 for shelf 1 and so on)

        public LevelData(int shelfCount, int rows, int columns) 
        { 
            //int time = 0;
            shelfPositiosn = new Vector3[shelfCount];
            items = new int[rows,columns];
            rowsForReconstruction = rows;
            columnsForReconstruction = columns;
        }
        public void InitializeLoadedLevel()
        {
            items = new int[rowsForReconstruction, columnsForReconstruction];
            ConvertToMultiDymensionArray();
        }



        public void ConvertToSingleDymensionArray()
        {
            int lenght = items.Length;
            arrayForSerialize = new int[lenght];
            int k  = 0;
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int j = 0; j < items.GetLength(1); j++)
                {
                    arrayForSerialize[k] = items[i,j];
                    k++;
                }
            }
        }

        public void ConvertToMultiDymensionArray()
        {
            int k = 0;
            totalNumberOfItems = 0;
            for (int i = 0; i < rowsForReconstruction; i++)
            {
                for (int j = 0; j < columnsForReconstruction; j++)
                {
                    items[i, j] = arrayForSerialize[k];
                    k++;
                    if(items[i, j] != 0)
                    {
                        totalNumberOfItems++;
                    }
                }
            }
        }
        public int[] ReturnItemArrayForShelf(int row)
        {
            int[] rowArray = new int[columnsForReconstruction];
            for (int i = 0; i < columnsForReconstruction; i++)
            {
                rowArray[i] = items[row, i];
            }
            return rowArray;
        }
    }
}
