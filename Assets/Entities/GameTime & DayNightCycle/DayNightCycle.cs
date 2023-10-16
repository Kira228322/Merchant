using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Volume))]
public class DayNightCycle : MonoBehaviour
{
    [SerializeField]private RawImage _farBackground;
    [SerializeField]private RawImage _nearBackground;
    private Color _white = new Color(0.9f,0.9f,0.9f);
    private Color _grey = new Color(0.8f,0.8f,0.8f);
    private Color _darkGrey = new Color(0.29f,0.29f,0.29f);
    private Color _black = new Color(0.23f,0.23f,0.23f);
    
    private Volume _volume;
    private Light2D _sun;
    private List<Light2D> _lights = new();
    private bool _activateLights;
    private WeatherController _weatherController;
    private float _rainWeightOffset; //Дождь плавно слегка затемняет экран
    private int _weatherStrength;

    private void Awake()
    {
        _weatherController = FindObjectOfType<WeatherController>();
        _volume = GetComponent<Volume>();
        _sun = GetComponent<Light2D>();
    }
    private void OnEnable()
    {
        GameTime.MinuteChanged += OnMinuteChanged;
        SceneManager.sceneLoaded += OnSceneChanged;
        _weatherController.WeatherStarted += OnWeatherStarted;
        _weatherController.WeatherFinished += OnWeatherFinished;
    }
    private void OnDisable()
    {
        GameTime.MinuteChanged -= OnMinuteChanged;
        SceneManager.sceneLoaded -= OnSceneChanged;
        _weatherController.WeatherStarted -= OnWeatherStarted;
        _weatherController.WeatherFinished -= OnWeatherFinished;
    }
    private void Start()
    {
        AdjustToCurrentTime();
    }
    private void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
    {
        _activateLights = false;
        _lights.Clear();
        _lights = FindObjectsOfType<Light2D>(true).Where(light => light.CompareTag("Toggleable Light")).ToList();
    }
    private void OnMinuteChanged()
    {
        AdjustToCurrentTime();
    }
    private void AdjustToCurrentTime()
    {
        float volumeWeight = 0;
        float cosineValue;
        switch (GameTime.Hours)
        {
            case int n when n >= 22 || n <= 2:
                volumeWeight = 1;
                break;
            case int n when n >= 3 && n <= 10:
                cosineValue = ((GameTime.Hours-3) * 60 + GameTime.Minutes) / 540f; // изменяется от 0 до 1, когда время изменяется от 3 до 12
                volumeWeight = Mathf.Cos(cosineValue * Mathf.PI) * 0.5f + 0.5f; // см f(x) = cos(Px)/2 + 0.5f
                break;
            case int n when n >= 11 && n <= 16:
                volumeWeight = 0;
                break;
            case int n when n >= 17 && n <= 21:
                cosineValue = 1 - ((GameTime.Hours - 17) * 60 + GameTime.Minutes) / 300f; // изменяется от 1 до 0, когда время изменяется от 17 до 22
                volumeWeight = Mathf.Cos(cosineValue * Mathf.PI) * 0.5f + 0.5f;
                break;
        }
        
        _volume.weight = volumeWeight + _rainWeightOffset;
        _sun.intensity = Mathf.Lerp(0.16f, 0.96f, 1-volumeWeight); // 0.16 и 0.96 это min и max значения которыми может быть освещение
        _nearBackground.color = Color.Lerp(_white, _darkGrey, volumeWeight);
        _farBackground.color = Color.Lerp(_grey, _black, volumeWeight);
        
        if (_volume.weight >= 1) _volume.weight = 1;
        if (!_activateLights)
        {
            if (volumeWeight >= 0.75f)
            {
                foreach (var light in _lights)
                {
                    light.transform.gameObject.SetActive(true);
                }
                _activateLights = true;
            }
        }
        else
        {
            if (volumeWeight <= 0.56f)
            {
                foreach (var light in _lights)
                {
                    light.transform.gameObject.SetActive(false);
                }
                _activateLights = false;
            }
        }
    }
    private void OnWeatherStarted()
    {
        GameTime.MinuteChanged += IncreaseWeatherWeightOffset;
        _weatherStrength = Convert.ToInt32(_weatherController.WeatherStrength) + 1;
    }
    private void OnWeatherFinished()
    {
        GameTime.MinuteChanged += DecreaseWeatherWeightOffset;
    }
    private void IncreaseWeatherWeightOffset()
    {
        _rainWeightOffset += _weatherStrength/100f;
        if (_rainWeightOffset >= _weatherStrength/10f)
        {
            _rainWeightOffset = _weatherStrength/10f;
            GameTime.MinuteChanged -= IncreaseWeatherWeightOffset;
        }
    }
    private void DecreaseWeatherWeightOffset()
    {
        _rainWeightOffset -= _weatherStrength/100f;
        if (_rainWeightOffset <= 0)
        {
            _rainWeightOffset = 0;
            GameTime.MinuteChanged -= DecreaseWeatherWeightOffset;
        }
    }
}