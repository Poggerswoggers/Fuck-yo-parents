using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image confirmScreen;

    public void LevelSelect() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void Settings() {
        SceneManager.LoadScene("Settings");
    }

    public void BackToMain() {
        SceneManager.LoadScene("MainMenu");
    }

    public void StoryMode(string sceneName) {
        Debug.Log("Click");
        SceneManager.LoadScene(sceneName);
    }

    public void ResetAll() {
        confirmScreen.gameObject.SetActive(true);
    }

    public void DenyReset() {
        confirmScreen.gameObject.SetActive(false);
    }

    public void ClearLevels() {
        //PlayerPrefs.DeleteAll();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        confirmScreen.gameObject.SetActive(false);
    }

    public void Unload(string SceneToUnload)
    {
        SceneManager.UnloadSceneAsync(SceneToUnload);
    }
}
