using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] List<Button> levels;
    [SerializeField] LevelDataScriptable[] levelScriptables;

    void Start()
    {
        SetUnlockedLevels();
    }

    void SetUnlockedLevels()
    {
        foreach(LevelDataScriptable levelData in levelScriptables)
        {
            if (levelData.unlocked)
            {
                Debug.Log("unlock");
                levels[levelData.levelIndex - 1].interactable = true;
            }
            else
            {
                Debug.Log("Lock");
                levels[levelData.levelIndex - 1].interactable = false;
                Debug.Log(levels[levelData.levelIndex-1].gameObject);
            }
        }
    }
}
