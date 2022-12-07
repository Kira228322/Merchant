using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransiter : MonoBehaviour
{
    [SerializeField] private Image _loadingBar;
    [SerializeField] private TMP_Text _loadingText;
    [SerializeField] private TravelTimeCounter _timeCounter;
    public TravelTimeCounter TimeCounter => _timeCounter;
    private Animator _animator;
    private AsyncOperation _loadingSceneOperation;
    private Road _road;
    private GameObject _travelBlock;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        enabled = false;
    }

    public void StartTransit(string scene, Road road)
    {
        _road = road;
        enabled = true;
        _animator.SetTrigger("StartTransition");
        
        
        _loadingSceneOperation = SceneManager.LoadSceneAsync(scene);
        _loadingSceneOperation.allowSceneActivation = false;
    }
    
    public void StartTransit(string scene)
    {
        _road = null;
        enabled = true;
        _animator.SetTrigger("StartTransition");

        _loadingSceneOperation = SceneManager.LoadSceneAsync(scene);
        _loadingSceneOperation.allowSceneActivation = false;
    }

    private void Update()
    {
        _loadingText.text = "Loading... " + Mathf.RoundToInt(_loadingSceneOperation.progress * 100) + "%";
        _loadingBar.fillAmount = _loadingSceneOperation.progress;
        if (_loadingSceneOperation.isDone)
        {
            _animator.SetTrigger("EndTransition");
            if (_road != null)
            {
                MapManager.StartTravel(_road);
            }
            enabled = false;
        }
    }

    public void OnAnimationOver()
    {
        _loadingSceneOperation.allowSceneActivation = true;
        if (_road != null)
        {
            GameTime.SetTimeScale(GameTime.TimeScaleInTravel);
            _travelBlock.SetActive(true);
            _travelBlock.GetComponent<Animator>().SetTrigger("StartTravel");
        }
    }
}
