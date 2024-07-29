using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingBtn : MonoBehaviour  {
    public Image image; 
    public float fadeSpeed = 0.5f; 
    public float minAlpha = 0f; 
    public float maxAlpha = 1f; 

    private bool fadingIn = true; 

    void Start() {
        if (image == null) {
            image = GetComponent<Image>();
        }

        if (image != null) {
            Color color = image.color;
            color.a = minAlpha;
            image.color = color;
        }
    }

    void Update() {
        if (image != null) {
            Color currentColor = image.color;
            float alphaChange = fadeSpeed * Time.deltaTime;

            if (fadingIn) {
                currentColor.a += alphaChange;
                if (currentColor.a >= maxAlpha) {
                    currentColor.a = maxAlpha;
                    fadingIn = false;
                }
            } else {
                currentColor.a -= alphaChange;
                if (currentColor.a <= minAlpha) {
                    currentColor.a = minAlpha;
                    fadingIn = true;
                }
            }

            image.color = currentColor;
        }
    }
}