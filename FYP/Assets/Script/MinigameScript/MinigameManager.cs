using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    [TextArea(2,5)]
    public string instructionText;
    [SerializeField] TextMeshProUGUI instructionTextField;
    private void OnEnable()
    {
        HeadTilt.OnGameOver += GameOver;
    }
    private void OnDisable()
    {
        HeadTilt.OnGameOver -= GameOver;
    }

    private void Start()
    {
        StartCoroutine(LoadInstructionCo());
    }

    IEnumerator LoadInstructionCo()
    {
        instructionTextField.text = instructionText;
        yield return new WaitForSeconds(1f);
        instructionTextField.text = "";
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

}
