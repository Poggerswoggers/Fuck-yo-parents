using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveSystem : MonoBehaviour
{
    [SerializeField] string jsonFilePath = "/LevelData.json";



    public void SaveLevelData()
    {
        //string saveData = JsonUtility.ToJson(s, true);
        //string filepath = Application.persistentDataPath + jsonFilePath;
        //System.IO.File.WriteAllText(filepath, saveData);
        Debug.Log("level data saved");
    }
}


