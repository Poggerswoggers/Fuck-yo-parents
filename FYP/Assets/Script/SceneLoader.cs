using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image confirmScreen;
    [SerializeField] AudioClip clickSound; 

    public void Awake() {
        }

    public void LevelSelect() {
        AudioManager.instance.PlaySFX(clickSound);
        SceneManager.LoadScene("LevelSelect");
    }

    public void Credits() {
        AudioManager.instance.PlaySFX(clickSound);
        SceneManager.LoadScene("Credits");
    }

    public void Settings() {
        AudioManager.instance.PlaySFX(clickSound);
        SceneManager.LoadScene("Settings");
    }

    public void BackToMain() {
        AudioManager.instance.PlaySFX(clickSound);
        SceneManager.LoadScene("MainMenu");
    }

    public void StoryMode(string sceneName) {
        Debug.Log("Click");
        SceneManager.LoadScene(sceneName);
    }

    public void ResetAll() {
        AudioManager.instance.PlaySFX(clickSound);
        confirmScreen.gameObject.SetActive(true);
    }

    public void DenyReset() {
        AudioManager.instance.PlaySFX(clickSound);
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
