using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialStep
    {
        Pan,
        Snap,
        Talk,
    }
    private TutorialStep currentStep;

    [SerializeField] Sprite checkedBox;
    [SerializeField] List<GameObject> tutorialSequence;
    [SerializeField] GameObject tweenObject;
    int index;
    bool canPerformAction = true;
    [SerializeField] GameObject tutorialPanel;

    [Header("Pan Test")]
    [SerializeField] Collider2D tutorialCommuter;
    [SerializeField] Transform reticle;
    [SerializeField] float minDragTime;

    
    float currentDragTime;
    Vector3 lastMousePosition;

    private void Start()
    {
        lastMousePosition = reticle.position;
        SetActiveSprite();
    }
    private void Update()
    {
        if (!canPerformAction) return;
        switch (currentStep)
        {
            case TutorialStep.Pan:
                Pan();
                break;
            case TutorialStep.Snap:
                Snap();
                break;
            case TutorialStep.Talk:
                break;
        }

    }

    void Pan()
    {
        if (Input.GetMouseButton(1))
        {
            if (reticle.position != lastMousePosition)
            {
                if (currentDragTime < minDragTime)
                {
                    currentDragTime += Time.deltaTime;
                }
                else
                {
                    StrikeAndCheck();
                    currentStep = TutorialStep.Snap;
                }
            }
            lastMousePosition = reticle.position;
        }
    }

    void Snap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StrikeAndCheck();
            tutorialCommuter.enabled = true;
            currentStep = TutorialStep.Talk;
        }
    }

    void StrikeAndCheck()
    {
        if (index > tutorialSequence.Count) return;
        canPerformAction = false;

        LTSeq sequence = LeanTween.sequence();
        tutorialSequence[index].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        tutorialSequence[index].GetComponentInChildren<Image>().sprite = checkedBox;
        index++;
        sequence.append(LeanTween.moveX(tweenObject.GetComponent<RectTransform>(), 600, 1.5f).setEaseInBack().setOnComplete(SetActiveSprite));
        sequence.append(LeanTween.moveX(tweenObject.GetComponent<RectTransform>(), -23, 0.8f));
    }

    void SetActiveSprite()
    {
        Debug.Log("set sprite");
        for(int i =0; i < tutorialSequence.Count; i++)
        {
            tutorialSequence[i].SetActive((i == index));
        }
        canPerformAction = true;
    }

    void Talk(){
        StrikeAndCheck();
    }


    private void OnEnable()
    {
        SnapCamera.SnapAction += Talk;
    }

    private void OnDisable()
    {
        SnapCamera.SnapAction -= Talk;
    }
}
