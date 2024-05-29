using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    private void OnEnable()
    {
        HeadTilt.OnGameOver += GameOver;
    }
    private void OnDisable()
    {
        HeadTilt.OnGameOver -= GameOver;
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

}
