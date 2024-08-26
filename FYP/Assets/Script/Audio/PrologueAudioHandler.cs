using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueAudioHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip levelMusic;

    private void OnEnable()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayMusic(levelMusic);
    }

    private void OnDisable()
    {
        LevelManager.Instance.PlayLevelAudio();
    }
}
