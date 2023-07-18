using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class WeatherController : MonoBehaviour, IEventController<GlobalEvent_Weather>, ISaveable<WeatherControllerSaveData>
{
    [SerializeField] private ParticleSystem _rain;
    public enum StrengthOfWeather {Light, Medium, Heavy}
    private StrengthOfWeather _strengthOfWeather;
    public StrengthOfWeather WeatherStrength => _strengthOfWeather;

    public int LastEventDay { get; set; }

    public int DateOfNextEvent { get; set; }
    public int HourOfNextEvent { get; set; }
    public int DurationOfEvent { get; set; }

    public int MinDelayToNextEvent => 3;
    public int MaxDelayToNextEvent => 9;
    

    public event UnityAction WeatherStarted;
    public event UnityAction WeatherFinished;

    void Start()
    {
        PredictNextEvent();
    }

    public void StartWeather()
    {
        _rain.Play();
        switch (_strengthOfWeather)
        {
            case StrengthOfWeather.Light:
                SetRainParams(35, 6.5f, 0.11f);
                DurationOfEvent = Random.Range(3, 12);
                break;
            case StrengthOfWeather.Medium:
                SetRainParams(80, 7f, 0.14f);
                DurationOfEvent = Random.Range(4, 15);
                break;
            case StrengthOfWeather.Heavy:
                SetRainParams(145, 7.7f, 0.16f);
                DurationOfEvent = Random.Range(3, 8);
                break;
        }
        WeatherStarted?.Invoke();
        _rain.transform.rotation = Quaternion.Euler(0,0, -Random.Range(10, 16));
    }



    public void PredictNextEvent()
    {
        DateOfNextEvent = LastEventDay + Random.Range(MinDelayToNextEvent, MaxDelayToNextEvent + 1);
        HourOfNextEvent = Random.Range(1, 24); // на всякий случай от 1 до 24, а не от 0 до 24, тк как хз
        // как там просиходит событие, когда меняется день. Тонкая штука. См метод CheckDayDelayToRainfall
        _strengthOfWeather = (StrengthOfWeather)Random.Range(0, Enum.GetNames(typeof(StrengthOfWeather)).Length);
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

    public GlobalEvent_Weather AddEvent()
    {
        GlobalEvent_Weather eventToAdd = GlobalEventHandler.Instance
            .AddGlobalEvent<GlobalEvent_Weather>(DurationOfEvent);
        eventToAdd.StrengthOfWeather = (int)_strengthOfWeather;
        LastEventDay = GameTime.CurrentDay;
        StartWeather();
        return eventToAdd;
    }
    public void RemoveEvent()
    {
        _rain.Stop();
        PredictNextEvent();
        WeatherFinished?.Invoke();
    }

    public WeatherControllerSaveData SaveData()
    {
        WeatherControllerSaveData saveData = new(LastEventDay, DateOfNextEvent, HourOfNextEvent, (int)_strengthOfWeather);
        return saveData;
    }

    public void LoadData(WeatherControllerSaveData data)
    {
        LastEventDay = data.LastEventDay;
        DateOfNextEvent = data.DateOfPrecipitation;
        HourOfNextEvent = data.HourOfPrecipitation;
        _strengthOfWeather = (StrengthOfWeather)data.StrengthOfWeather;
    }
}
