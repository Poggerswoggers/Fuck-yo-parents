using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image confirmScreen;

    AudioManager audioManager;
    public Animator animator;
    public Animator disabledAnimator;

    public void Awake(){
        if (AudioManager.instance)
        {
            audioManager = AudioManager.instance;
        }

        if(SceneManager.GetActiveScene().name == "Prologue") {
            StartCoroutine(WaitForCutscene());
        }
    }

    public void LevelSelect()
    {
        
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            audioManager?.PlaySFX(audioManager.camSnap);
            if (animator != null)
            {
                animator.enabled = true;
                animator.SetTrigger("out");
            }
            if (disabledAnimator != null)
            {
                disabledAnimator.enabled = false;
            }
            StartCoroutine(WaitForScene());
        }
        else
        {
            audioManager?.PlaySFX(audioManager.buttonClick);
            SceneManager.LoadScene("LevelSelect");
        }


    }

    public void Prologue()
    {
        audioManager?.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene("Prologue");
    }

    public void Credits() {
        audioManager?.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene("Credits");
    }

    public void Settings() {
        audioManager?.PlaySFX(audioManager.buttonClick);
    }

    public void BackToMain()
    {
        Time.timeScale = 1;
        audioManager?.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene("MainMenu");
    }

    public void StoryMode(string sceneName) {
        audioManager?.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ResetAll() {
        audioManager?.PlaySFX(audioManager.buttonClick);
        confirmScreen.gameObject.SetActive(true);
    }

    public void DenyReset() {
        audioManager?.PlaySFX(audioManager.buttonClick);
        confirmScreen.gameObject.SetActive(false);
    }

    public void ClearLevels(SaveSystem saveScriptable)
    {
        saveScriptable.ResetData();
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

    IEnumerator WaitForScene()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("LevelSelect");
    }

    IEnumerator WaitForCutscene()
    {
        yield return new WaitForSeconds(25.5f);
        SceneManager.LoadScene("Tutorial");
    }
}
