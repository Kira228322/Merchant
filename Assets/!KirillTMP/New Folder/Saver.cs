using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour
{
    [SerializeField] private SaveData _saveData;
    private void Start()
    {
        SceneManager.sceneLoaded += LoadData;
        SceneManager.sceneUnloaded += SaveData;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadData;
        SceneManager.sceneUnloaded -= SaveData;
    }

    private void LoadData(Scene scene, LoadSceneMode sceneMode)
    {
        Player.Singleton.LoadData(_saveData.Player);
    }

    private void SaveData(Scene scene)
    {
        Player.Singleton.SaveData(_saveData);
    }
    
    
}
