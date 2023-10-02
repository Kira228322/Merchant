using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransiter : MonoBehaviour, ISaveable<SceneSaveData>
{
    [SerializeField] private Image _loadingBar;
    [SerializeField] private TMP_Text _loadingText;
    [SerializeField] private Toggle _mapButton;
    
    private Animator _animator;
    private AsyncOperation _loadingSceneOperation;
    private Road _road;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    private void Start()
    {
        enabled = false;
    }

    public void StartTransit(string scene, Road road)
    {
        _mapButton.isOn = false;
        _road = road;
        MapManager.TravelInit(_road);
        
        enabled = true;
        _animator.SetTrigger("StartTransition");
        
        
        _loadingSceneOperation = SceneManager.LoadSceneAsync(scene);
        _loadingSceneOperation.allowSceneActivation = false;
        MapManager.IsActiveSceneTravel = true;
    }
    
    public void StartTransit(Location location)
    {
        _mapButton.isOn = false;
        _road = null;
        enabled = true;
        _animator.SetTrigger("StartTransition");

        _loadingSceneOperation = SceneManager.LoadSceneAsync(location.SceneName);
        _loadingSceneOperation.allowSceneActivation = false;
        MapManager.IsActiveSceneTravel = false;
        MapManager.CurrentLocation = location;
    }

    private void Update()
    {
        _loadingText.text = "Loading... " + Mathf.RoundToInt(_loadingSceneOperation.progress * 100) + "%";
        _loadingBar.fillAmount = _loadingSceneOperation.progress;
        if (_loadingSceneOperation.isDone)
        {
            enabled = false;
        }
    }
    
    public void OnAnimationOver()
    {
        _loadingSceneOperation.allowSceneActivation = true;
        if (_road != null)
        {
            GameTime.SetTimeScale(GameTime.TimeScaleInTravel);
        }
        else
        {
            MapManager.PlayerIcon.transform.position = MapManager.CurrentLocation.transform.position;
        }
        _animator.SetTrigger("EndTransition");
    }

    public void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        //TODO: ≈сли заходить с главного меню по кнопке, то этот метод не триггеритс€.
        //≈сли загрузка срабатывает после поездки, то триггеритс€. “ак происходит потому что успевает сделать себе в Update
        //enabled = false, соответственно срабатывает OnDisable и этот метод уже не триггеритс€
        //Ќо почему в разных случа€х разное поведение, и как должно быть?
        //(28.09.23) я добавл€ю авто-сохранение здесь, а не там, потому что здесь работает, а там не работает.
        //я не знаю почему и не смог сам разобратьс€.
        if (_road == null)
        {
            GameManager.Instance.SaveGame();
            MapManager.OnLocationChange();
        }
    }

    public SceneSaveData SaveData()
    {
        SceneSaveData saveData = new(SceneManager.GetActiveScene().name);
        return saveData;
    }

    public void LoadData(SceneSaveData data)
    {
        Location currentLocation = MapManager.GetLocationBySceneName(data.sceneName);
        StartTransit(currentLocation);
    }
}
