using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, ISaveable<GlobalSaveData>
{
    public static GameManager Instance;
    [SerializeField] private Canvas _canvas;
    [Header("MapManager")] 
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _sceneTransiter;
    [SerializeField] private GameObject _roadWindow;
    [SerializeField] private GameObject _villageWindow;
    [SerializeField] private GameObject _playerIcone;
    [SerializeField] private Location _startLocation;

    [Header("GameTime")] 
    [FormerlySerializedAs("Timeflow")] [SerializeField] private Timeflow _timeflow;

    [Header("Regions & Scenes")]
    [SerializeField] private RegionHandler _regionHandler;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _loadGameButton;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        
        MapManager.Init(_travelingScene, _sceneTransiter, _roadWindow, _villageWindow, _canvas, _playerIcone, _startLocation);
        GameTime.Init(_timeflow);

        _regionHandler.InitializeAll(); //Регионы подсосут из текстовика и создадут словари

        _newGameButton.onClick.AddListener(StartNewGame);
        _loadGameButton.onClick.AddListener(LoadGame);

        if (File.Exists(Application.persistentDataPath + "/GlobalSave.data")) //Если нету сейва то кнопка продолжить будет неактивной
        {
            _loadGameButton.interactable = true;
        }
        else _loadGameButton.interactable = false;
    }

    public void StartNewGame() //По нажатию кнопки Новая игра в MainMenu
    {
        _sceneTransiter.StartTransit(_startLocation);
        _newGameButton.onClick.RemoveListener(StartNewGame);
    }
    public void LoadGame() //По нажатию кнопки Продолжить в MainMenu
    {
        LoadData(SaveLoadSystem<GlobalSaveData>.LoadData("GlobalSave"));
        _loadGameButton.onClick.RemoveListener(LoadGame);
    }
    public void SaveGame()
    {
        SaveLoadSystem<GlobalSaveData>.SaveData(SaveData(), "GlobalSave");
    }

    public GlobalSaveData SaveData()
    {
        GlobalSaveData saveData = new()
        {
            PlayerData = Player.Instance.SaveData(),
            QuestSaveData = QuestHandler.SaveQuests(),
            NpcDatabaseSaveData = NpcDatabase.SaveNPCs(),
            TimeFlowSaveData = GameTime.SaveData(),
            CooldownHandlerSaveData = FindObjectOfType<CooldownHandler>().SaveData(),
            GlobalEventHandlerSaveData = GlobalEventHandler.Instance.SaveData(),
            RegionSaveData = _regionHandler.SaveData(),
            SceneSaveData = _sceneTransiter.SaveData(),
        };
        return saveData;
    }

    public void LoadData(GlobalSaveData data)
    {
        Player.Instance.LoadData(data.PlayerData);
        QuestHandler.LoadQuests(data.QuestSaveData);
        NpcDatabase.LoadNPCs(data.NpcDatabaseSaveData);
        GameTime.LoadData(data.TimeFlowSaveData);
        FindObjectOfType<CooldownHandler>().LoadData(data.CooldownHandlerSaveData);
        GlobalEventHandler.Instance.LoadData(data.GlobalEventHandlerSaveData);
        _regionHandler.LoadData(data.RegionSaveData);
        _sceneTransiter.LoadData(data.SceneSaveData);
    }
}
