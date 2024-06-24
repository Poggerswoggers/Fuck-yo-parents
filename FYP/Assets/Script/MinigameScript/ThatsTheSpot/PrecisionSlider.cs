using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PrecisionSlider : MonoBehaviour
{
    Slider slider;

    [SerializeField] AnimationCurve speedCurve;

    bool sliderActive = true;
    bool reverse;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        reverse = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!sliderActive) return;   
        float curveValue = speedCurve.Evaluate(slider.value);
        float deltaValue = curveValue * Time.fixedDeltaTime;

        slider.value += (!reverse) ? deltaValue : -deltaValue;
        if (slider.value >= slider.maxValue || slider.value <= slider.minValue)
        {
            // Reverse the direction by multiplying deltaValue by -1
            reverse = !reverse;
            Debug.Log(reverse);
        }
    }

    public float GetSliderPoint()
    {
        sliderActive = false;
        return slider.value;
    }
}
    