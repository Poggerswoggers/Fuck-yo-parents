using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    public Slider timerSlider;
    public float gameTime;


    // Update is called once per frame

    private void Start()
    {
        timerSlider.maxValue = gameTime;
    }

    void Update()
    {

        if (gameTime <=0)
        {
            gameTime = 0;
        }
        else
        {
            gameTime -= Time.deltaTime;
        }
        timerSlider.value = gameTime;
    }
}
