using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public GameObject gameOverScreen;

    private void OnEnable()
    {
        Timer.OnTimeFinish += GameOver;
    }
    private void OnDisable()
    {
        Timer.OnTimeFinish -= GameOver;
    }
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        gameOverScreen.transform.SetAsLastSibling();
    }
}
