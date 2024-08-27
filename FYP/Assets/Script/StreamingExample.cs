using Prologue;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class StreamingExample : MonoBehaviour
{
    [SerializeField] string videoFileName;
    [SerializeField] string videoFileName2;
    public VideoPlayer videoPlayer;
    DialoguePrologue dialogue;
    bool hasDone;
    private void Start()
    {
        hasDone = false;
        dialogue = FindObjectOfType<DialoguePrologue>();
        PlayVideo();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void PlayVideo()
    {
        if(videoPlayer != null)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }

    public void PlayVideo2()
    {
        if (videoPlayer != null)
        {

            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName2);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        if(SceneManager.GetActiveScene().name == "Ending")
        {
            if (!hasDone)
            {
                hasDone = true;
                dialogue.PlayStory();
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}