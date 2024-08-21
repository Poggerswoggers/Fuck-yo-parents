using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelData/Save System")]
public class SaveSystem : ScriptableObject
{
    [SerializeField] string jsonFilePath = "/LevelData.json";
    public LevelDataScriptable levelData;

    public void SaveLevelData()
    {
        string saveData = JsonUtility.ToJson(levelData, true);
        string filepath = Application.persistentDataPath + jsonFilePath;
        System.IO.File.WriteAllText(filepath, saveData);
        Debug.Log("level data saved");
    }

    public void RetrieveLevelData()
    {
        string filePath = Application.persistentDataPath + jsonFilePath;
        string _levelData = System.IO.File.ReadAllText(filePath);

        // Populate the existing ScriptableObject instance with the data
        JsonUtility.FromJsonOverwrite(_levelData, levelData);
        Debug.Log("Data loaded");
    }

    public void ResetData()
    {
        foreach(LevelData ld in levelData.levelDataArray)
        {
            ld.unlocked = false;
            ld.Score = 0;
        }
        SaveLevelData();
    }
}


