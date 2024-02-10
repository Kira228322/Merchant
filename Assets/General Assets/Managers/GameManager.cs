using System;
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
    public GameObject Camera;
    public AudioSource _ButtonAudioSource;
    [SerializeField] private GameObject _GameIsSavedText;
    [SerializeField] private Canvas _canvas;
    public CanvasGroup PlayerBlock;
    public GameObject ButtonsBlock;
    [HideInInspector] public GameObject CurrentFunctionalWindow;
    [HideInInspector] public GameObject CurrentWarningWindow;
    public UIClock UIClock;

    [SerializeField] private Menu _optionsMenu;

    [Header("MapManager")] 
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _sceneTransiter;
    [SerializeField] private GameObject _roadWindow;
    [SerializeField] private GameObject _villageWindow;
    [SerializeField] private GameObject _playerIcone;
    [SerializeField] private Location _startLocation;
    [HideInInspector] public List<int> IndexesLastEvents = new ();

    //TODO убрать перед выпуском игры
    [SerializeField] private TMP_InputField TESTstartLocationInputField;
    [SerializeField] private Button TESTstartNewGameButton;
    //end TODO убрать перед выпуском игры

    [Header("GameTime")] 
    [FormerlySerializedAs("Timeflow")] [SerializeField] private Timeflow _timeflow;

    [Header("Regions & Scenes")]
    [SerializeField] private RegionHandler _regionHandler;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button _loadGameButton;

    private void OnEnable()
    {
        Application.targetFrameRate = 100;
        _sceneTransiter.EnteredVillageScene += SaveGame;
    }
    private void OnDisable()
    {
        _sceneTransiter.EnteredVillageScene -= SaveGame;
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        //TODO убрать перед выпуском игры
        TESTstartNewGameButton.onClick.AddListener(() => StartNewGame(TESTstartLocationInputField.text));
        //end TODO убрать перед выпуском игры

        Application.targetFrameRate = 100;

        MapManager.Init(_travelingScene, _sceneTransiter, _roadWindow, _villageWindow, _canvas, _playerIcone, _startLocation);
        GameTime.Init(_timeflow);
        GameTime.SetTimeScale(13);
        GameTime.TimeSet(0, 7, 30);
        GlobalEventHandler.Instance.Initialize();

        _regionHandler.InitializeAll(); //–егионы подсосут из текстовика и создадут словари

        if (File.Exists(Application.persistentDataPath + "/GlobalSave.data")) //≈сли нету сейва то кнопка продолжить будет неактивной
            _loadGameButton.interactable = true;
        else _loadGameButton.interactable = false;

        _optionsMenu.LoadData(); //«агрузка из PlayerPrefs настроек игрока
    }

    private void EnableUI()
    {
        PlayerBlock.alpha = 1;
        PlayerBlock.interactable = true;

        CanvasGroup CanvasGroupButtonBlock = ButtonsBlock.GetComponent<CanvasGroup>();
        CanvasGroupButtonBlock.alpha = 1;
        CanvasGroupButtonBlock.interactable = true;

        UIClock.Refresh();
        CanvasGroup CanvasGroupUIClock = UIClock.GetComponent<CanvasGroup>();
        CanvasGroupUIClock.alpha = 1;
        CanvasGroupUIClock.interactable = true;
    }
    
    //TODO убрать перед выпуском игры
    public void StartNewGame(string startingSceneName)
    {
        Player.Instance.Statistics.OnToughnessChanged();
        GameTime.SetTimeScale(1);
        Player.Instance.Needs.CurrentHunger = Player.Instance.Needs.MaxHunger;
        Player.Instance.Needs.CurrentSleep = Player.Instance.Needs.MaxSleep;
        _sceneTransiter.StartTransit(MapManager.GetLocationBySceneName(startingSceneName));

        GlobalEventHandler.Instance.ResetEvents();

        GameTime.TimeSet(1, 8, 0);
        EnableUI();
    }
    //end TODO убрать перед выпуском игры
    public void StartNewGame() //ѕо нажатию кнопки Ќова€ игра в MainMenu
    {
        Player.Instance.Statistics.OnToughnessChanged();
        GameTime.SetTimeScale(1);
        Player.Instance.Needs.CurrentHunger = Player.Instance.Needs.MaxHunger;
        Player.Instance.Needs.CurrentSleep = Player.Instance.Needs.MaxSleep;
        _sceneTransiter.StartTransit(_startLocation);

        GlobalEventHandler.Instance.ResetEvents();

        GameTime.TimeSet(1, 8, 0);
        EnableUI();
    }
    public void LoadGame() //ѕо нажатию кнопки ѕродолжить в MainMenu
    {
        GameTime.SetTimeScale(1);

        GlobalEventHandler.Instance.ResetEvents(); //¬ажно, что это происходит до загрузки

        LoadData(SaveLoadSystem<GlobalSaveData>.LoadData("GlobalSave"));
        Player.Instance.Statistics.OnToughnessChanged();
        EnableUI();
    }
    public void SaveGame()
    {
        SaveLoadSystem<GlobalSaveData>.SaveData(SaveData(), "GlobalSave");
        _GameIsSavedText.SetActive(true);
    }

    public GlobalSaveData SaveData()
    {

        GlobalSaveData saveData = new()
        {
            PlayerData = Player.Instance.SaveData(),
            TutorialTrackerSaveData = FindObjectOfType<TutorialStateTracker>().SaveData(),
            StatusManagerSaveData = StatusManager.Instance.SaveData(),
            JournalSaveData = new(),
            BannedItemsSaveData = FindObjectOfType<BannedItemsHandler>().SaveData(),
            NpcDatabaseSaveData = NpcDatabase.SaveNPCs(),
            RestockSaveData = GetComponent<RestockHandler>().SaveData(),
            TimeFlowSaveData = GameTime.SaveData(),
            CooldownHandlerSaveData = FindObjectOfType<CooldownHandler>().SaveData(),
            GlobalEventHandlerSaveData = GlobalEventHandler.Instance.SaveData(),
            RegionSaveData = _regionHandler.SaveData(),
            SceneSaveData = _sceneTransiter.SaveData(),
            TravelEventsSaveData = new(IndexesLastEvents),

            };
        return saveData;
    }

    public void LoadData(GlobalSaveData data)
    {
        //TODO: (просто информаци€ дл€ размышлени€, но надо убрать в конце)
        //Ќа момент 14.09.23 по€вилась разница в пор€дке загрузки:
        //TutorialTrackerSaveData должен загружатьс€ ƒќ JournalSaveData (т.е QuestSaveData)
        //потому что иначе TutorialTracker покажет презентацию, прежде чем узнает что она уже была показана.
        // онечно это не страшно, но довольно примечательно что до этого ничего не зависело от пор€дка загрузки

        Player.Instance.LoadData(data.PlayerData);
        FindObjectOfType<TutorialStateTracker>().LoadData(data.TutorialTrackerSaveData);
        StatusManager.Instance.LoadData(data.StatusManagerSaveData);
        QuestHandler.LoadQuests(data.JournalSaveData.QuestsSaveData);
        Diary.Instance.LoadData(data.JournalSaveData.DiarySaveData);
        NpcDatabase.LoadNPCs(data.NpcDatabaseSaveData);
        GetComponent<RestockHandler>().LoadData(data.RestockSaveData);
        GameTime.LoadData(data.TimeFlowSaveData);
        FindObjectOfType<CooldownHandler>().LoadData(data.CooldownHandlerSaveData);
        GlobalEventHandler.Instance.LoadData(data.GlobalEventHandlerSaveData);
        _regionHandler.LoadData(data.RegionSaveData);
        _sceneTransiter.LoadData(data.SceneSaveData);
        IndexesLastEvents = new(data.TravelEventsSaveData.SavedIndexes);

    }
}
