using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransiter : MonoBehaviour
{
    [SerializeField] private GameObject _map;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private TMP_Text _loadingText;
    
    private Animator _animator;
    private AsyncOperation _loadingSceneOperation;
    private Road _road;
    
    

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
        _map.SetActive(false);
        _road = road;
        MapManager.TravelInit(_road);
        
        enabled = true;
        _animator.SetTrigger("StartTransition");
        
        
        _loadingSceneOperation = SceneManager.LoadSceneAsync(scene);
        _loadingSceneOperation.allowSceneActivation = false;
    }
    
    public void StartTransit(string scene)
    {
        _map.SetActive(false);
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
    }
}
