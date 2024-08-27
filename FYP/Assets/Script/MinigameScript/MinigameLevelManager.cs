using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameLevelManager : MonoBehaviour
{
    public static MinigameLevelManager Instance { get; private set; }
    public static int selectedMinigameDifficulty = 1;
    protected MinigameSceneLoader sceneLoader;
    protected int levelScore;

    [SerializeField] GameObject minigameSelectCanvas;
    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        sceneLoader = new MinigameSceneLoader();
    }

    public virtual void LoadAddictiveScene(string sceneName, int difficulty)
    {
        selectedMinigameDifficulty = difficulty;
        minigameSelectCanvas.SetActive(false);
        sceneLoader.LoadAddictiveScene(sceneName);
    }

    public virtual void UnloadAddictiveScene(int score)
    {
        sceneLoader.UnloadAddictiveScene();
        minigameSelectCanvas.SetActive(true);
    }

    public void LoadMinigameOnClick(string sceneName)
    {
        Debug.Log("hehe");
    }

    public virtual int GetLevelScore() {  return levelScore; }
}
