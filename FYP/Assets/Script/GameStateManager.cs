using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{    
    public NpcStateManager nSm { get; set; }

    
    GameBaseState currentState;

    [Header("Game State")]
    public SnapCamera snapState;
    public DialogueManager dialogueStat;
    public McqManager mcqState;

    [Header("Audio")]
    [SerializeField] AudioClip levelMusic;
    private void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic(levelMusic);
        }
        currentState = snapState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        if(currentState !=null)
        {
            currentState.UpdateState(this);
        }
    }

    public void ChangeStat(GameBaseState state)
    {
        currentState.gameObject.SetActive(false);
        currentState = state;
        currentState.gameObject.SetActive(true);
        state.EnterState(this);
    }

    public void ClearNpc()
    {
        Destroy(nSm.gameObject);
    }
}
