using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReaderScreen : MonoBehaviour
{
    SpriteRenderer sr;

    [SerializeField] Sprite correctScreen;
    [SerializeField] Sprite wrongScreen;
    [SerializeField] Sprite holdScreen;
    [SerializeField] Sprite tapScreen;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = tapScreen;
    }

    public void ScreenReader (bool passed)
    {
        StartCoroutine(ScreenReaderCo(passed));
    }

    IEnumerator ScreenReaderCo(bool passed)
    {
        sr.sprite = (passed) ? correctScreen : wrongScreen;
        yield return new WaitForSeconds(0.5f);
        sr.sprite = holdScreen;
        yield return new WaitForSeconds(1f);
        sr.sprite = tapScreen;
    }
}
