using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheatPanelOpener : MonoBehaviour
{
    // Update is called once per frame

    [SerializeField] GameObject panel;

    [SerializeField] LevelDataScriptable scriptable;
    [SerializeField] SaveSystem save;

    private KeyCode[] keys = new KeyCode[]
    {
        KeyCode.C,
        KeyCode.O,
        KeyCode.M,
        KeyCode.M,
        KeyCode.U,
        KeyCode.T,
        KeyCode.E,
        KeyCode.R,
        KeyCode.C,
        KeyCode.H,
        KeyCode.A,
        KeyCode.M,
        KeyCode.P,
        KeyCode.I,
        KeyCode.O,
        KeyCode.N,
        KeyCode.S,
    };

    public bool success = false;

    IEnumerator Start()
    {
        float timer = 0f;
        int index = 0;

        while (!success)
        {
            if (Input.GetKeyDown(keys[index]))
            {
                index++;

                if (index == keys.Length)
                {
                    success = true;
                    timer = 0f;
                }
                else
                {
                    timer = 0.4f;
                }
            }

            timer -= Time.deltaTime;
            if(timer < 0f)
            {
                timer = 0;
                index = 0;
            }

            yield return null;
        }
    }
    void Update()
    {
        if (success)
        {
            panel.SetActive(true);
        }
   
    }

    public void UpdateLevel(int level)
    {
        level -= 1;

        Debug.Log(level);
        LevelData levelData = scriptable.levelDataArray[level];
        levelData.unlocked = true;
        levelData.Score = 1000000000;

        save.SaveLevelData();
    }

    public void MiniGameUnlock()
    {
        scriptable.unlocked = true;
        save.SaveLevelData();
    }

    public void UnlockAllLevels()
    {
        foreach(var level in scriptable.levelDataArray)
        {
            level.unlocked = true;
            level.Score = 1000000000;
        }
        save.SaveLevelData();
    }

    public void ClosePanel()
    {
        success = false;
        panel.SetActive(false);
        StartCoroutine(Start());
    }
}
