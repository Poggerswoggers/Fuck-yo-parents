using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameAudioHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip levelMusic;

    private void Awake()
    {
        AudioManager.instance.StopSFX(AudioManager.instance.mrtMove);
    }

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
