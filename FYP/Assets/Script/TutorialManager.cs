using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialStep
    {
        Pan,
        Snap,
        UI,
        Talk,
    }
    public TutorialStep currentStep;

    [SerializeField] GameObject gm;

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

    [Header("UI Test")]
    [SerializeField] GameObject uiTestParent;
    int uiTestCount;


    float currentDragTime;
    Vector3 lastMousePosition;

    private void Start()
    {
        LeanTween.delayedCall(0.3f,LevelManager.Instance.DisableLevelUI);
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
                tutorialCommuter.enabled = true;
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
                    currentDragTime += Time.deltaTime;             
                else
                {
                    StrikeAndCheck();
                    UICheck();
                    currentStep = TutorialStep.UI;
                }
            }
            lastMousePosition = reticle.position;
        }
    }

    void UICheck()
    {
        LevelManager.Instance.EnableLevelUI();
        LeanTween.delayedCall(1,()=> uiTestParent.SetActive(true));
    }

    public void CheckUI()
    {
        StrikeAndCheck();
        uiTestParent.SetActive(false);
        currentStep = TutorialStep.Talk;
    }
    void Snap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StrikeAndCheck();
            currentStep = TutorialStep.Pan;
        }
    }

    void StrikeAndCheck()
    {
        if (index > tutorialSequence.Count - 1) return;
        canPerformAction = false;

        LTSeq sequence = LeanTween.sequence();
        tutorialSequence[index].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        tutorialSequence[index].GetComponentInChildren<Image>().sprite = checkedBox;
        index++;
        sequence.append(LeanTween.moveX(tweenObject.GetComponent<RectTransform>(), 600, 1.5f).setEaseInBack().setOnComplete(SetActiveSprite));
        sequence.append(LeanTween.moveX(tweenObject.GetComponent<RectTransform>(), -23, 0.8f).setOnComplete(() => canPerformAction = true));
    }

    void SetActiveSprite()
    {
        for (int i = 0; i < tutorialSequence.Count; i++)
        {
            tutorialSequence[i].SetActive((i == index));
        }
    }

    void Talk()
    {
        Debug.Log("strike check talk");
        StrikeAndCheck();
        SnapCamera.SnapAction -= Talk;
        SnapCamera.SnapAction += Finish;
    }

    void Finish()
    {
        Debug.Log("strike check finish");
        LevelManager.Instance.EndLevel();
    }
    private void OnEnable()
    {
        SnapCamera.SnapAction += Talk;
    }

    private void OnDisable()
    {
        SnapCamera.SnapAction = null;
    }
}
