using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{ 
    public GameObject promptBox;
    [SerializeField] float promptBoxYPos;

    public GameObject infoBox;
    [SerializeField] float infoBoxXPos;

    [SerializeField] float phaseInSpeed;

    public void LoadDialogue()
    {
        LeanTween.moveY(promptBox.GetComponent<RectTransform>(), promptBoxYPos, phaseInSpeed);
        LeanTween.moveX(infoBox.GetComponent<RectTransform>(), infoBoxXPos, phaseInSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
