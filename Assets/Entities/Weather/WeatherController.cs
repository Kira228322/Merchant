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

    private int _minDelayToNextPrecipitation = 3;
    private int _maxDelayToNextPrecipitation = 9;
    
    private int _durationOfWeatherInHours;

    private int _dateOfPrecipitation;
    private int _hourOfPrecipitation;

    public event UnityAction WeatherStarted;
    public event UnityAction WeatherFinished;

    void Start()
    {
        PredictNextPrecipitation();
    }

    private void OnEnable()
    {
        GameTime.DayChanged += CheckDayDelayToPrecipitation;
    }

    private void OnDisable()
    {
        GameTime.DayChanged -= CheckDayDelayToPrecipitation;
    }

    public void StartWeather()
    {
        _rain.Play();
        switch (_strengthOfWeather)
        {
            case StrengthOfWeather.Light:
                SetRainParams(35, 6.5f, 0.11f);
                _durationOfWeatherInHours = Random.Range(3, 12);
                break;
            case StrengthOfWeather.Medium:
                SetRainParams(80, 7f, 0.14f);
                _durationOfWeatherInHours = Random.Range(4, 15);
                break;
            case StrengthOfWeather.Heavy:
                SetRainParams(145, 7.7f, 0.16f);
                _durationOfWeatherInHours = Random.Range(3, 8);
                break;
        }
        WeatherStarted?.Invoke();
        _rain.transform.rotation = Quaternion.Euler(0,0, -Random.Range(10, 16));
    }

    public void StopWeather()
    {
        _rain.Stop();
        PredictNextPrecipitation();
        GameTime.DayChanged += CheckDayDelayToPrecipitation;
        WeatherFinished?.Invoke();
    }

    private void CheckDayDelayToPrecipitation()
    {
        if (GameTime.CurrentDay >= _dateOfPrecipitation)
        {
            GameTime.HourChanged += CheckHourDelayToPrecipitation;
            GameTime.DayChanged -= CheckDayDelayToPrecipitation;
        }
    }

    private void CheckHourDelayToPrecipitation()
    {
        if (GameTime.Hours >= _hourOfPrecipitation)
        {
            GameTime.HourChanged -= CheckHourDelayToPrecipitation;

            AddEvent();
            StartWeather();
        }
    }

    private void PredictNextPrecipitation()
    {
        _dateOfPrecipitation = LastEventDay + Random.Range(_minDelayToNextPrecipitation, _maxDelayToNextPrecipitation + 1);
        _hourOfPrecipitation = Random.Range(1, 24); // на всякий случай от 1 до 24, а не от 0 до 24, тк как хз
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
            .AddGlobalEvent<GlobalEvent_Weather>(_durationOfWeatherInHours);
        eventToAdd.StrengthOfWeather = (int)_strengthOfWeather;
        LastEventDay = GameTime.CurrentDay;
        return eventToAdd;
    }

    public WeatherControllerSaveData SaveData()
    {
        WeatherControllerSaveData saveData = new(LastEventDay, _dateOfPrecipitation, _hourOfPrecipitation, (int)_strengthOfWeather);
        return saveData;
    }

    public void LoadData(WeatherControllerSaveData data)
    {
        LastEventDay = data.LastEventDay;
        _dateOfPrecipitation = data.DateOfPrecipitation;
        _hourOfPrecipitation = data.HourOfPrecipitation;
        _strengthOfWeather = (StrengthOfWeather)data.StrengthOfWeather;
    }
}
