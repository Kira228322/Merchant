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

    public event UnityAction EnteredVillageScene;
    public event UnityAction EnteredTravelScene;

    private Animator _animator;
    private AsyncOperation _loadingSceneOperation;
    private Road _road;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
        enabled = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
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
        if (_road == null)
        {
            MapManager.OnLocationChange();
            EnteredVillageScene?.Invoke();
        }
        else
            EnteredTravelScene?.Invoke();
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
