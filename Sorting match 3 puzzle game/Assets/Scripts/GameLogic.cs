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
    public Text scoreText;
    public Text multiplierText;
    public Text comboText;
    public GameObject lastLevelForNow;
    public int numberOfItemsLeft;

    private int score = 0;
    private int multiplier = 1;
    private float combo = 0;
    private float timeLeft = 0;
    private float maxTime = 8f;

    private const float minTime = 3.5f;

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
        combo++;
        if (maxTime - combo > minTime)
        {
            timeLeft = maxTime - combo;
        }
        else
        {
            timeLeft = minTime;
        }
        if (combo == 3)
        {
            multiplier = 2;
            multiplierText.text = string.Format("x{0}", multiplier);
        }
        else if (combo == 6)
        {
            multiplier = 3;
            multiplierText.text = string.Format("x{0}", multiplier);
        }
        
        comboText.text = string.Format("{0}", combo);
        score += 100*multiplier;
        scoreText.text = string.Format("{0}", score);

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
    private void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

        }
        else
        {
            multiplier = 1;
            combo = 0;
        }
    }


}
