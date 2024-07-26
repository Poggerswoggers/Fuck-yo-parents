using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.EventSystems;

public class RouteAssister : BaseMiniGameClass
{
    [Header("Route Assister")]
    //The starting map
    [SerializeField] GameObject map;

    //Bus sequence button
    [SerializeField] GameObject busOptionPanels;
    List<TextMeshProUGUI> busTexts = new List<TextMeshProUGUI>();

    //The player's direction
    [SerializeField] GameObject directorPanel;
    List<string> numberAlpha = new List<string> { "1st", "2nd", "3rd", "4th"}; //A list to store string
    public int clear = 0; //Number of correct option to match with scriptable

    //Ask Panel
    [SerializeField] GameObject askPanel;
    
    [Header("Scriptable Object List")]
    [SerializeField] List<Destinations> destinationsScriptable;  //A list of all routes

    Destinations currentDes; //current route
    int index = 0;  //route index

    [Header("Button")]
    [SerializeField] List<Button> routeButton;

    [Header("npc")]
    [SerializeField] Transform npc;

    [Header("Cinemachine ref")]
    [SerializeField] CinemachineVirtualCamera cam;   //2 cinemachine cam for smooth transition
    [SerializeField] CinemachineVirtualCamera mapCam;

    public override void EndSequenceMethod()
    {
        base.UnloadedAndUpdateScore(score);
    }

    public override void StartGame()
    {
        destinationsScriptable = Helper.Shuffle(destinationsScriptable);    //Shuffle the list
        score = 2000;  //suvject to change

        StartCoroutine(StartSequence());
        isGameActive = true;    //Set is game active
        LeanTween.reset();
    }
    IEnumerator StartSequence()
    {
        clear = 0;
        index = 0;

        currentDes = destinationsScriptable[0];  //Sets current route as the first scriptable in the list
        destinationsScriptable.RemoveAt(0);

        //Cool transition sequence
        map.GetComponent<SpriteRenderer>().sprite = currentDes.map;
        map.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        mapCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        cam.m_Lens.OrthographicSize = 6;
        mapCam.gameObject.SetActive(false);
        map.SetActive(false);

        LeanTween.moveX(cam.gameObject, 5.6f, 0.5f).setDelay(1f);

        yield return new WaitForSeconds(1.5f);
        SetGame();  //Set the path
        SetButtons();
    }

    void SetGame()
    {
        askPanel.SetActive(true);
        askPanel.GetComponentInChildren<TextMeshProUGUI>().text = $"How can I get to <u><color=green> {currentDes.destinationName}</u></color>?";  

        for (int i =0; i<currentDes.route.Count; i++)   //Set the text active and have the blanks
        {
            GameObject _text = directorPanel.transform.GetChild(i).gameObject;
            _text.SetActive(true);
            _text.GetComponent<TextMeshProUGUI>().text = numberAlpha[i];
             TextMeshProUGUI busText = _text.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            busText.text = ":<u>         .</u>";

            busTexts.Add(busText);
        }
        directorPanel.SetActive(true);
        busOptionPanels.SetActive(true);
    }
    //This sets all the option button text and onclick to possess a int 
    void SetButtons()
    {
        int index = 0;
        List<Destinations.MRTRoutes> allRoutes = Enum.GetValues(typeof(Destinations.MRTRoutes)).Cast<Destinations.MRTRoutes>().ToList();
        foreach (Button button in routeButton)
        {
            //Set int to string and int to the onclick
            Destinations.MRTRoutes buttonRoute = allRoutes[index];
            button.GetComponent<Button>().onClick.AddListener(() => OnClickBusNumber(buttonRoute));
            EventTrigger buttonEvent = button.GetComponent<EventTrigger>();
            buttonEvent.AddListener(EventTriggerType.PointerEnter, EnterHover);
            buttonEvent.AddListener(EventTriggerType.PointerExit, ExitHover);
            index++;
        }
    }
    void ResetButton()
    {
        foreach(Button button in routeButton)
        {
            button.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public override void UpdateGame()
    {

    }

    protected override IEnumerator InstructionCo()
    {
        //throw new System.NotImplementedException();
        StartGame();
        yield return null;
    }

    public void OnClickBusNumber(Destinations.MRTRoutes buttonRoute)
    {
        clear = (currentDes.route[index] == buttonRoute) ? clear + 1 : clear;   //Clear goes up when selected button int matches the current route index int
        busTexts[index].text = $": <u><color=green>{buttonRoute}</color></u>";       //Set rich text color

        if(index == currentDes.route.Count-1)   //if index = 2 means filled out route choices
        {
            CheckIfMatch();
            if (destinationsScriptable.Count>0)
            {
                StartCoroutine(NextRound());
            }
            else
            {
                gameManager.OnGameOver();
            }
        }
        index++;
    }
    IEnumerator NextRound()
    {
        busOptionPanels.SetActive(false);
        directorPanel.SetActive(false);
        askPanel.SetActive(false);
        LeanTween.moveX(cam.gameObject, 0f, 0.5f).setDelay(1f);
        //Reset button listener
        ResetButton();    

        yield return new WaitForSeconds(2f);
        StartCoroutine(StartSequence());
    }

    void CheckIfMatch()
    { 
        if(clear == currentDes.route.Count){
            score += 500;
        }
        else{
            score -= 500;
        }
    }

    void EnterHover(PointerEventData eventData)
    {
        LeanTween.scale(eventData.pointerEnter, Vector3.one*0.95f, 0.2f);
    }
    void ExitHover(PointerEventData eventData)
    {
        float facing = eventData.pointerEnter.transform.lossyScale.x;
        LeanTween.scale(eventData.pointerEnter, Vector3.one*0.9f, 0.2f);
    }
}
