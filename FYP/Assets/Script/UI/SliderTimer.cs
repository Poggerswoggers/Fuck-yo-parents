using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SliderTimer : MonoBehaviour
{
    [SerializeField] Slider timerSlider;
    private float gameTime;

    private Action timerCallback;



    // Update is called once per frame
    void Update()
    {
        if(gameTime>0f)
        {
            gameTime -= Time.deltaTime;

            if(isTimesUp()){
                timerCallback?.Invoke();
            }
            timerSlider.value = gameTime;
        }
    }

    public void SetTImer(float gameTIme, Action timerCallback)
    {
        this.gameTime = gameTIme;
        timerSlider.maxValue = gameTime;
        this.timerCallback = timerCallback;
    }

    public void TimePenalty(float penaltyTime)
    {
        gameTime -= penaltyTime;
    }

    public bool isTimesUp()
    {
        return gameTime < 0f;
    }
}
