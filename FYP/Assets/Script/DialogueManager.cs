using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    //Typing Speed
    [SerializeField] float typingSpeed;

    [SerializeField] TextAsset inkStoryJson;
    private Story inkStory;

    [SerializeField] TextMeshProUGUI infoTextField;
    [SerializeField] TextMeshProUGUI nameField;
    
    [Header("UI Panel")]
    public GameObject promptBox;
    [SerializeField] float promptBoxYPos;
    public GameObject infoBox;
    [SerializeField] float infoBoxXPos;
    [SerializeField] float phaseInSpeed;


    private Coroutine displayLineCoroutine;

    //Loads and tweens the dialogue boxes;
    public void LoadDialooguePanel() 
    {
        LeanTween.moveY(promptBox.GetComponent<RectTransform>(), promptBoxYPos, phaseInSpeed).setDelay(0.3f);
        LeanTween.moveX(infoBox.GetComponent<RectTransform>(), infoBoxXPos, phaseInSpeed);

        StartStory();
    }

    void StartStory()
    {
        inkStory = new Story(inkStoryJson.text);
        DisplayNewLine();
    }

    void DisplayNewLine()
    {
        if (inkStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string infoText = inkStory.Continue();
            infoText = infoText?.Trim();

            displayLineCoroutine = StartCoroutine(DisplayNextLineEffect(infoText));
        }
    }

    IEnumerator DisplayNextLineEffect(string infoText)
    {
        infoTextField.text = "";
        //CanContinue

        yield return new WaitForSeconds(0.3f);
        foreach (char letter in infoText.ToCharArray())
        {
            infoTextField.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        DisplayChoices();
    }

    void DisplayChoices()
    {
        if(inkStory.currentChoices.Count < 0) return;
        

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
