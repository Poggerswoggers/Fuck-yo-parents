using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaderScreen : MonoBehaviour
{
    SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ScreenReader (bool passed)
    {
        StartCoroutine(ScreenReaderCo(passed));
    }

    IEnumerator ScreenReaderCo(bool passed)
    {
        sr.color = (passed) ? Color.green : Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }
}
