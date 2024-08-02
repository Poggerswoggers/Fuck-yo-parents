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
        audioManager.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene("LevelSelect");
    }

    public void Credits() {
        audioManager.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene("Credits");
    }

    public void Settings() {
        audioManager.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene("Settings");
    }

    public void BackToMain() {
        audioManager.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene("MainMenu");
    }

    public void StoryMode(string sceneName) {
        audioManager.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene(sceneName);
    }

    public void ResetAll() {
        audioManager.PlaySFX(audioManager.buttonClick);
        confirmScreen.gameObject.SetActive(true);
    }

    public void DenyReset() {
        audioManager.PlaySFX(audioManager.buttonClick);
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
