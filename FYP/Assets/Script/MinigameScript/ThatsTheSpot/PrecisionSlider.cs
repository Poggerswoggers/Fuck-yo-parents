using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrecisionSlider : MonoBehaviour
{
    Slider slider;

    [SerializeField] AnimationCurve speedCurve;

    public float duration = 3f;
    public float elapseTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float curveValue = speedCurve.Evaluate(slider.value);

        slider.value += Time.deltaTime * (curveValue +1);
    }
}
