using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MinigameSceneLoader
{
    string addictiveSceneName;

    public void UnloadAddictiveScene()
    {
        SceneManager.UnloadSceneAsync(addictiveSceneName);
    }

    public void LoadAddictiveScene(string sceneName)
    {
        addictiveSceneName = sceneName;        
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
