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
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, $"{saveName}.data");
        using (var stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static T LoadData(string saveName)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{saveName}.data");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
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
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return default;
        }
    }

    public static void SavePlayer()
    {
        PlayerData saveData = Player.Instance.SaveData();
        SaveLoadSystem<PlayerData>.SaveData(saveData, "PlayerSave");
    }
    public static void LoadPlayer()
    {
        PlayerData saveData = SaveLoadSystem<PlayerData>.LoadData("PlayerSave");
        Player.Instance.LoadData(saveData);
    }

}