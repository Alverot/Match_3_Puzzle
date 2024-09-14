using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float timeLeft;
    public GameObject victoryScreen;

    public static event Action OnTimeFinish;
    void Update()
    {
        bool sendEventOnce = false;
        if (!victoryScreen.activeSelf)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;

            }
            else
            {
                timeLeft = 0;
                if (timeLeft == 0 && sendEventOnce == false)
                {
                    OnTimeFinish?.Invoke();
                    sendEventOnce = true;
                }
            }

            if (timeLeft < 0)
            {
                timeLeft = 0;
            }
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
    }
}
