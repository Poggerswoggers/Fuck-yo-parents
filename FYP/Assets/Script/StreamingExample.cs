using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class StreamingExample : MonoBehaviour
{
    [SerializeField] string videoFileName;
    public VideoPlayer videoPlayer;
    private void Start()
    {
        PlayVideo();
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
}