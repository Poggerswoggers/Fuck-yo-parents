using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    [SerializeField] Slider timerSlider;
    private float gameTime;

    bool start;

    BaseMiniGameClass bmGC;

    // Update is called once per frame
    void Update()
    {
        if (!start) return;  

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

    public void SetValues(float gameTIme)
    {
        this.gameTime = gameTIme;
        timerSlider.maxValue = gameTime;

        start = true;
    }
}
