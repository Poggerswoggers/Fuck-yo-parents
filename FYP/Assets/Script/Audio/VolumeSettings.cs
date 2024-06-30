using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start() {
        // Load player prefs for volume
        if (PlayerPrefs.HasKey("musicVolume")) {
            LoadVolume();
        }
        else {
            // Set new value
            SetBGMVolume();
            SetSFXVolume();
        }
    }

    public void SetBGMVolume() {
        float volume = bgmSlider.value;
        myMixer.SetFloat("bgm", Mathf.Log10(volume) * 20); // Change the min volume to 0.0001
        PlayerPrefs.SetFloat("musicVolume", volume);
        }

    public void SetSFXVolume() {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20); // Change the min volume to 0.0001
        PlayerPrefs.SetFloat("sfxVolume", volume);
        }

    private void LoadVolume() {
        bgmSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetBGMVolume();
        SetSFXVolume();
    }
}
