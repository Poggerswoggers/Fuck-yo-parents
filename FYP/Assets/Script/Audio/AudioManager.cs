using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;

    [Header("Audio source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Menu ")]
    [SerializeField] AudioClip menuBGM;

    [Header("SFX source")]
    public AudioClip camSnap;
    public AudioClip buttonClick;

    public AudioClip npcSad;
    public AudioClip npcHappy;

    public AudioClip mrtPass;
    public AudioClip mrtMove;

    public AudioClip lvlDone;
    public AudioClip mgDone;

    public AudioClip cardTap;
    public AudioClip wrongCardTap;
    public AudioClip cardSwoosh;

    public AudioClip pmdDrift;

    public AudioClip percentageCalc;

    public AudioClip wrongGrid;
    public AudioClip correctGrid;

    public AudioClip attachJig;
    public AudioClip dettachJig;
    public AudioClip correctJig;

    public AudioClip npcHmm;

    public static AudioManager instance;
    private bool isPaused = false;

    public void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float music = PlayerPrefs.GetFloat("musicVolume");
        float sfx = PlayerPrefs.GetFloat("sfxVolume");

        myMixer.SetFloat("bgm", Mathf.Log10(music) * 20); // Change the min volume to 0.0001
        myMixer.SetFloat("sfx", Mathf.Log10(sfx) * 20); // Change the min volume to 0.0001

        SceneManager.activeSceneChanged += OnSceneChanged;

        // Play bgm once app launches
        /*
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            musicSource.clip = menuBGM;
            musicSource.Play();
        }
        */
    }

    public void PlayMenuBGM()
    {
        if (musicSource.clip != menuBGM || !musicSource.isPlaying)
        {
            musicSource.clip = menuBGM;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f) 
    {
        if (!isPaused)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void StopSFX(AudioClip clip)
    {
        if (!isPaused)
        {
            sfxSource.Stop();
            sfxSource.clip = clip;
        }
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // Reset the pause state when the scene changes
        SetPauseState(false);
        StopSFX(mrtPass);
    }

    public void SetPauseState(bool paused)
    {
        isPaused = paused;
        if (isPaused)
        {
            sfxSource.Pause();
        }
        else
        {
            sfxSource.UnPause();
        }
    }
}
