using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadSystem<T>
{
    public static void SaveData(T data, string saveName)
    {
        BinaryFormatter formatter = new();
        string path = Path.Combine(Application.persistentDataPath, $"{saveName}.data");
        using FileStream stream = new(path, FileMode.Create);
        formatter.Serialize(stream, data);
    }

    public static T LoadData(string saveName)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{saveName}.data");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(path, FileMode.Open);
            try
            {
                T data = (T)formatter.Deserialize(stream);
                return data;
            }
            // deserialization failed (probably user tampered with the file?)
            catch (SerializationException)
            {
                return default;
            }
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return default;
        }
    }

    public static void SaveAll()
    {
        PlayerData playerSaveData = Player.Instance.SaveData();
        SaveLoadSystem<PlayerData>.SaveData(playerSaveData, "PlayerSave");
        QuestSaveData questSaveData = QuestHandler.SaveQuests();
        SaveLoadSystem<QuestSaveData>.SaveData(questSaveData, "QuestsSave");
    }
    public static void LoadAll()
    {
        PlayerData playerSaveData = SaveLoadSystem<PlayerData>.LoadData("PlayerSave");
        Player.Instance.LoadData(playerSaveData);
        QuestSaveData questSaveData = SaveLoadSystem<QuestSaveData>.LoadData("QuestsSave");
        QuestHandler.LoadQuests(questSaveData);
    }

}