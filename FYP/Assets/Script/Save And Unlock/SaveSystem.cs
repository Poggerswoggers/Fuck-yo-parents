using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelData/Save System")]
public class SaveSystem : ScriptableObject
{
    public LevelDataScriptable levelData;

    public void SaveLevelData()
    {
        string saveData = JsonUtility.ToJson(levelData, true);
        PlayerPrefs.SetString("LevelDataKey", saveData);
        PlayerPrefs.Save(); // Make sure the data is saved to IndexedDB
        Debug.Log("level data saved");
    }

    public void RetrieveLevelData()
    {
        if (PlayerPrefs.HasKey("LevelDataKey"))
        {
            // Retrieve the JSON string from PlayerPrefs
            string jsonData = PlayerPrefs.GetString("LevelDataKey");
            // Deserialize the JSON string back into the LevelData object
            JsonUtility.FromJsonOverwrite(jsonData, levelData);
            Debug.Log("Data loaded from IndexedDB.");
        }
    }

    public void ResetData()
    {
        foreach(LevelData ld in levelData.levelDataArray)
        {
            ld.unlocked = false;
            ld.Score = 0;
        }
        levelData.unlocked = false;
        SaveLevelData();
    }
}


