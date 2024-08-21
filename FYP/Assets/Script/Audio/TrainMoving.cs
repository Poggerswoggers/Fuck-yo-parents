using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainMoving : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        // SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void TrainMove()
    {
        if (Time.timeScale > 0 && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.mrtPass, 0.2f);
        }
    }
    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop playing the sound when a new scene is loaded
        AudioManager.instance?.StopSFX(AudioManager.instance.mrtPass);
    }
    */
}
