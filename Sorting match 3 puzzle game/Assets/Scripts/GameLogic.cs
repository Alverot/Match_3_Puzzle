using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public Text levelIndex;
    public GameObject lastLevelForNow;
    public int numberOfItemsLeft;

    private void OnEnable()
    {
        Timer.OnTimeFinish += GameOver;
        ShelfLogic.OnValidMatch += WinConditionTraking;
    }
    private void OnDisable()
    {
        Timer.OnTimeFinish -= GameOver;
        ShelfLogic.OnValidMatch -= WinConditionTraking;
    }
    private void WinConditionTraking()
    {
        if (numberOfItemsLeft > 0) 
        {
            int numberOfItemsOnAShelf = 3;
            numberOfItemsLeft -= numberOfItemsOnAShelf;
        }
        
        if (numberOfItemsLeft == 0)
        {
            victoryScreen.SetActive(true);
            if ("5" == levelIndex.text)
            {
                lastLevelForNow.SetActive(true);
            }
        }
    }
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        if(numberOfItemsLeft > 0)
        {
            gameOverScreen.SetActive(true);
            gameOverScreen.transform.SetAsLastSibling();
        }

    }
}
