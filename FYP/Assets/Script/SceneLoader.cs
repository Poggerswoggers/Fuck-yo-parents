using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image confirmScreen;

    AudioManager audioManager;

    public void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }

    public void LevelSelect() {
        audioManager.PlaySFX(audioManager.clickSound);
        SceneManager.LoadScene("LevelSelect");
    }

    public void Credits() {
        audioManager.PlaySFX(audioManager.clickSound);
        SceneManager.LoadScene("Credits");
    }

    public void Settings() {
        audioManager.PlaySFX(audioManager.clickSound);
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

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
