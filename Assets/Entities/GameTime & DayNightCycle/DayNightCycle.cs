using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Volume))]
public class DayNightCycle : MonoBehaviour
{
    private Volume _volume;
    private List<Light2D> _lights = new();
    private bool _activateLights;
    private WeatherController _weatherController;
    private float _rainWeightOffset; //Дождь плавно слегка затемняет экран

    private void Awake()
    {
        _weatherController = FindObjectOfType<WeatherController>();
        _volume = GetComponent<Volume>();
    }
    private void OnEnable()
    {
        GameTime.MinuteChanged += OnMinuteChanged;
        SceneManager.sceneLoaded += OnSceneChanged;
        _weatherController.RainStarted += OnRainStarted;
        _weatherController.RainFinished += OnRainFinished;
    }
    private void OnDisable()
    {
        GameTime.MinuteChanged -= OnMinuteChanged;
        SceneManager.sceneLoaded -= OnSceneChanged;
        _weatherController.RainStarted -= OnRainStarted;
        _weatherController.RainFinished -= OnRainFinished;
    }
    private void Start()
    {
        AdjustToCurrentTime();
    }
    private void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
    {
        _lights.Clear();
        _lights = FindObjectsOfType<Light2D>(true).ToList(); //Получить список выключаемых светильников на новой сцене
        _lights.Remove(GetComponent<Light2D>()); //кроме себя
    }
    private void OnMinuteChanged()
    {
        AdjustToCurrentTime();
    }
    private void AdjustToCurrentTime()
    {
        float cosineValue = (GameTime.Hours * 60 + GameTime.Minutes) / 1500f; //в дне максимум 1499 минут
        float volumeWeight = Mathf.Cos(cosineValue * 2.0f * Mathf.PI) * 0.5f + 0.5f; //см.график функции f(x) = cos(2pi*x)/2 + 0.5
        _volume.weight = volumeWeight + _rainWeightOffset;
        if (_volume.weight >= 1) _volume.weight = 1;
        if (!_activateLights)
        {
            if (_volume.weight >= 0.75f)
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
            if (_volume.weight <= 0.56f)
            {
                foreach (var light in _lights)
                {
                    light.transform.gameObject.SetActive(false);
                }
                _activateLights = false;
            }
        }
    }
    private void OnRainStarted()
    {
        GameTime.MinuteChanged += IncreaseRainWeightOffset;
    }
    private void OnRainFinished()
    {
        GameTime.MinuteChanged += DecreaseRainWeightOffset;
    }
    private void IncreaseRainWeightOffset()
    {
        _rainWeightOffset += 0.03f;
        if (_rainWeightOffset >= 0.3f)
        {
            _rainWeightOffset = 0.3f;
            GameTime.MinuteChanged -= IncreaseRainWeightOffset;
        }
    }
    private void DecreaseRainWeightOffset()
    {
        _rainWeightOffset -= 0.03f;
        if (_rainWeightOffset <= 0)
        {
            _rainWeightOffset = 0;
            GameTime.MinuteChanged -= DecreaseRainWeightOffset;
        }
    }
}