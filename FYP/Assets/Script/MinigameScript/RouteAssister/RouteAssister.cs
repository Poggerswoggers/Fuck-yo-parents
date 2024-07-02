using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

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
    List<int> fakeRoute = new List<int> { 198, 502, 69 };
    public int clear = 0; //Number of correct option to match with scriptable

    //Ask Panel
    [SerializeField] GameObject askPanel;
    
    [Header("Scriptable Object List")]
    [SerializeField] List<Destinations> destinationsScriptable;  //A list of all routes

    Destinations currentDes; //current route
    public int index = 0;  //route index

    [Header("Button")]
    [SerializeField] List<Button> routeButton;

    [Header("npc")]
    [SerializeField] Transform npc;

    [Header("Rounds")]
    [SerializeField] int rounds;

    [Header("Cinemachine ref")]
    [SerializeField] CinemachineVirtualCamera cam;   //2 cinemachine cam for smooth transition
    [SerializeField] CinemachineVirtualCamera mapCam;

    public List<Destinations> ShuffleList(List<Destinations> list) //Fisher-Yates shuffle
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Destinations temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }
    public override void EndSequenceMethod()
    {
        base.UnloadedAndUpdateScore(score);
    }

    public override void StartGame()
    {
        destinationsScriptable = ShuffleList(destinationsScriptable);    //Shuffle the list

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

        for (int i =0; i<currentDes.busRoutes.Count; i++)   //Set the text active and have the blanks
        {
            GameObject _text = directorPanel.transform.GetChild(i).gameObject;
            _text.SetActive(true);
            _text.GetComponent<TextMeshProUGUI>().text = numberAlpha[i];
             TextMeshProUGUI busText = _text.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            busText.text = ":<u>      .</u>";

            busTexts.Add(busText);
        }
        directorPanel.SetActive(true);
        busOptionPanels.SetActive(true);
    }
    //This sets all the option button text and onclick to possess a int 
    void SetButtons()
    {
        List<int> combinedList = currentDes.busRoutes.Concat(fakeRoute).ToList();
        foreach(Button button in routeButton)
        {
            int rand = Random.Range(0, combinedList.Count - 1); //Get rand
            int routeNum = combinedList[rand];
            button.GetComponentInChildren<TextMeshProUGUI>().text = routeNum.ToString();           //Set int to string and int to the onclick
            button.GetComponent<Button>().onClick.AddListener(() => onClickBusNumber(routeNum));
            combinedList.RemoveAt(rand);

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

    public void onClickBusNumber(int number)
    {
        clear = (currentDes.busRoutes[index] == number) ? clear + 1 : clear;   //Clear goes up when selected button int matches the current route index int
        busTexts[index].text = $":<u><color=green>{number}</color></u>";       //Set rich text color

        if(index == currentDes.busRoutes.Count-1)   //if index = 2 means filled out route choices
        {
            CheckIfMatch();
            if (rounds>0)
            {
                rounds--;
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
        if(clear == currentDes.busRoutes.Count){
            score += 500;
        }
        else{
            score -= 500;
        }
    }
}
