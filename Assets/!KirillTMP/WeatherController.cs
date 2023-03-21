using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _rain;
    private enum StrengthOfRain {MushroomRain, OrdinaryRain, Downpour}
    private StrengthOfRain _strengthOfRain;
    
    private int _minDelayToNextRainfall = 3;
    private int _maxDelayToNextRainfall = 9;
    
    private int _durationOfRainfallInHours;

    private int _dateOfRainfall;
    private int _hourOfRainfall;
    
    void Start()
    {
        PredictNextRainfall();
        
        // Если хочешь протестировать как выглядит погода раскомменти строку снизу
        // StartRain();
    }

    private void OnEnable()
    {
        GameTime.DayChanged += CheckDayDelayToRainfall;
    }

    private void OnDisable()
    {
        GameTime.DayChanged -= CheckDayDelayToRainfall;
    }

    private void StartRain()
    {
        _rain.Play();
        // TODO потом можно еще менять глобальное освещение
        switch (_strengthOfRain)
        {
            case StrengthOfRain.MushroomRain:
                SetRainParams(35, 6.5f, 0.11f);
                _durationOfRainfallInHours = Random.Range(3, 12);
                break;
            case StrengthOfRain.OrdinaryRain:
                SetRainParams(80, 7f, 0.14f);
                _durationOfRainfallInHours = Random.Range(4, 15);
                break;
            case StrengthOfRain.Downpour:
                SetRainParams(145, 7.7f, 0.16f);
                _durationOfRainfallInHours = Random.Range(3, 8);
                break;
        }
        
        _rain.transform.rotation = Quaternion.Euler(0,0, -Random.Range(10, 16));
    }

    private void StopRain()
    {
        _rain.Stop();
        PredictNextRainfall();
        GameTime.DayChanged += CheckDayDelayToRainfall;
        GameTime.HourChanged -= DecreaseDuration;
    }

    private void DecreaseDuration()
    {
        _durationOfRainfallInHours--;
        if (_durationOfRainfallInHours <= 0)
            StopRain();
    }

    private void CheckDayDelayToRainfall()
    {
        if (_dateOfRainfall == GameTime.CurrentDay)
        {
            GameTime.HourChanged += CheckHourDelayToRainfall;
            GameTime.DayChanged -= CheckDayDelayToRainfall;
        }
    }

    private void CheckHourDelayToRainfall()
    {
        if (_hourOfRainfall == GameTime.Hours)
        {
            GameTime.HourChanged -= CheckHourDelayToRainfall;
            StartRain();
            GameTime.HourChanged += DecreaseDuration;
        }
    }
    
    

    private void PredictNextRainfall()
    {
        _dateOfRainfall = GameTime.CurrentDay + Random.Range(_minDelayToNextRainfall, _maxDelayToNextRainfall + 1);
        _hourOfRainfall = Random.Range(1, 24); // на всякий случай от 1 до 24, а не от 0 до 24, тк как хз
        // как там просиходит событие, когда меняется день. Тонкая штука. См метод CheckDayDelayToRainfall
        
        _strengthOfRain = (StrengthOfRain)Random.Range(0, Enum.GetNames(typeof(StrengthOfRain)).Length);
    }

    private void SetRainParams(int rateOverTime, float speed, float maxSizeOfDrop)
    {
        var emissionModule = _rain.emission;
        emissionModule.rateOverTime = rateOverTime;
        
        var mainModule = _rain.main;
        mainModule.startSpeed = speed;

        var mainModuleStartSize = mainModule.startSize;
        mainModuleStartSize.constantMax = maxSizeOfDrop;
        mainModule.startSizeMultiplier = maxSizeOfDrop;
    }
}
