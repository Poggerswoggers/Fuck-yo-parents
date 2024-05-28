using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] List<GameObject> dots;
    public LeanTweenType easeType;

    private void Start()
    {
        InvokeRepeating("dotAnim", 0, 3f);
        
    }
    void dotAnim()
    {
        StartCoroutine(dotAnimCo());
    }

    IEnumerator dotAnimCo()
    {
        LeanTween.reset();
        for(int i =0; i< dots.Count; i++)
        {
            LeanTween.moveLocalY(dots[i], 0.3f, 1f).setEase(easeType);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
