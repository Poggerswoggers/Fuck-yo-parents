using UnityEngine;
using UnityEngine.Audio;

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
    public AudioClip cardTap;
    public AudioClip wrongCardTap;
    public AudioClip buttonClick;

    public static AudioManager instance;

    public void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            }
        else {
            Destroy(gameObject);
            }
        }

    private void Start()
    {
        float music = PlayerPrefs.GetFloat("musicVolume");
        float sfx = PlayerPrefs.GetFloat("sfxVolume");

        myMixer.SetFloat("bgm", Mathf.Log10(music) * 20); // Change the min volume to 0.0001
        myMixer.SetFloat("sfx", Mathf.Log10(sfx) * 20); // Change the min volume to 0.0001

        // Play bgm once app launches
        musicSource.clip = menuBGM;
        musicSource.Play();
        }

    public void PlaySFX(AudioClip clip) 
    {        
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip){
        musicSource.clip = clip;
        musicSource.Play();
    }
}
