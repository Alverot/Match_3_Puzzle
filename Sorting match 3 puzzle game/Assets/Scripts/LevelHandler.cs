using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelHandler : MonoBehaviour
{
    private string levelsFolderPath;
    public GameObject timerObject;
    public GameObject shelfPrefab;
    public Transform parentForShelfTransform;

    public static event Action OnFinishLevelLoad;


    //positions for coordonate 0 in the canvas
    //"x": 960.0,
    //"y": 540.0,
    //"z": 0.0

    private void Start()
    {
        levelsFolderPath = Path.Combine(Application.dataPath, "Levels");  //note REMEMBER TO CHANGE THIS IN THE FUTURE
        /*
        LevelData level1 = new LevelData(3, 3, 3);
        level1.time = 30;
        level1.shelfPositiosn[0] = new Vector3(3, 4, 0);
        level1.shelfPositiosn[1] = new Vector3(1, 1, 0);
        level1.shelfPositiosn[2] = new Vector3(0, 9, 0);

        level1.items[0, 0] = 1;
        level1.items[0, 1] = 2;
        level1.items[0, 2] = 3;
        level1.items[1, 0] = 4;
        level1.items[1, 1] = 5;
        level1.items[1, 2] = 6;
        level1.items[2, 0] = 7;
        level1.items[2, 1] = 8;
        level1.items[2, 2] = 0;
        level1.ConvertToSingleDymensionArray();
        SaveLevel(level1, "LV_1.json");
        */
        LevelData level1 = LoadLevelDataFromFile("LV_1.json");
        level1.InitializeLoadedLevel();
        level1.ConvertToMultiDymensionArray();

        LoadLevelOnScreen(level1);
    }

    private void LoadLevelOnScreen(LevelData level)
    {
        Timer timer = timerObject.GetComponent<Timer>();
        timer.timeLeft = level.time;
        
        for(int i = 0; i < level.shelfPositiosn.Length; i++)
        {
            GameObject newestShelf  = Instantiate(shelfPrefab, level.shelfPositiosn[i], Quaternion.identity, parentForShelfTransform);
            
            ShelfLogic shelfLogic = newestShelf.GetComponent<ShelfLogic>();
            shelfLogic.itemLayerList = level.ReturnItemArrayForShelf(i);
        }
        OnFinishLevelLoad?.Invoke();
    }

    private void SaveLevel(LevelData level, string fileName)
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

    private class LevelData
    {
        public int time;
        public int totalNumberOfItems;
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
            for (int i = 0; i < rowsForReconstruction; i++)
            {
                for (int j = 0; j < columnsForReconstruction; j++)
                {
                    items[i, j] = arrayForSerialize[k];
                    k++;
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
