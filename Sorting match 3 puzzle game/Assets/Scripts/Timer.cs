using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timeLeft;

    // Update is called once per frame
    void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            
        }
        else
        {
            timeLeft = 0;
        }
        
        if(timeLeft < 0)
        {
            timeLeft = 0;
        }
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
