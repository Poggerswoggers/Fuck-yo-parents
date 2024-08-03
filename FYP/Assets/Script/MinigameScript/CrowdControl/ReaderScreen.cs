using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReaderScreen : MonoBehaviour
{
    SpriteRenderer sr;

    [Header("Screen")]
    [SerializeField] Sprite correctScreen;
    [SerializeField] Sprite wrongScreen;
    [SerializeField] Sprite holdScreen;
    [SerializeField] Sprite tapScreen;

    [Header("Audio")]
    [SerializeField] AudioClip correctTap;
    [SerializeField] AudioClip wrongTap;
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
        AudioManager.instance?.PlaySFX((passed)? correctTap : wrongTap);
        yield return new WaitForSeconds(0.5f);
        sr.sprite = holdScreen;
        yield return new WaitForSeconds(1f);
        sr.sprite = tapScreen;
    }
}
