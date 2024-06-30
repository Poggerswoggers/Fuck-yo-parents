using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip clickSound; 
    public AudioClip menuBGM;
    public AudioClip snapSound;

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

    private void Start() {
        // Play bgm once app launches
        musicSource.clip = menuBGM;
        musicSource.Play();
        }

    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
        }

    }
