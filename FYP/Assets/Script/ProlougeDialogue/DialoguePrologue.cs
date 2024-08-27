using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

namespace Prologue
{
    public class DialoguePrologue : MonoBehaviour
    {
        //Text asset
        [SerializeField] TextAsset inkStoryJson;
        private Story inkStory;

        [Header("UI Panel")]//This the the entire dialogue panel
        [SerializeField] GameObject dialoguePanel;
        [SerializeField] TextMeshProUGUI storyTextField;

        bool canContinueToNextLine = false;
        private Coroutine displayLineCoroutine;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (dialoguePanel.activeInHierarchy && inkStory != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (canContinueToNextLine)
                    {
                        DisplayNewLine();
                    }
                    else
                    {
                        StopAllCoroutines();
                        storyTextField.text = inkStory.currentText.Trim(); //Auto fills when mouse button 0 is pressed
                    }
                }
                if (storyTextField.text == inkStory.currentText.Trim()) //If the dialoguetext == story line text, display choices
                {
                    canContinueToNextLine = true;
                }
            }
        }

        void PlayStory()
        {
            dialoguePanel.SetActive(true);
            inkStory = new Story(inkStoryJson.text);

            DisplayNewLine();
        }

        void DisplayNewLine()   //Call to show next line
        {
            if (inkStory.canContinue)
            {
                if (displayLineCoroutine != null)
                    StopCoroutine(displayLineCoroutine);

                string nextLine = inkStory.Continue();
                while (Helper.IsNullOrWhiteSpace(nextLine))
                {
                    nextLine = inkStory.Continue();
                }
                nextLine = nextLine.Trim();
                displayLineCoroutine = StartCoroutine(DisplayNextLineEffect(nextLine));
            }
            else
            {
                //This area is called when the story has reached its end
            }
        }

        IEnumerator DisplayNextLineEffect(string infoText)
        {
            storyTextField.text = "";
            canContinueToNextLine = false;

            char[] textArray = infoText.ToCharArray();

            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < infoText.Length; i++)
            {
                if (textArray[i].Equals('<'))
                {
                    storyTextField.text += Helper.GetCompleteRichTextTag(ref i, textArray);
                }
                else
                {
                    storyTextField.text += textArray[i];
                }
                yield return new WaitForSeconds(0.02f); //This number here controls tyyping speed, lower = faster
            }
        }
    }
}
