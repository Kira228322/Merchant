using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalSaveData 
{
    private static void SaveAll()
    {
        GlobalSaveData globalSaveData = new();

        SaveLoadSystem<GlobalSaveData>.SaveData(globalSaveData, "Save");
    }
}
