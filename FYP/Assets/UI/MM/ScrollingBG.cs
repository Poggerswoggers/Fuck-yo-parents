using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBG : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _X, _Y;
     
    // Update is called once per frame
    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_X, _Y) * Time.deltaTime, _img.uvRect.size);
    }
}
