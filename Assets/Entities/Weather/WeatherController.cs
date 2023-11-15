using System;
using System.Collections;
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
    public int MaxDelayToNextEvent => 7;
    

    public event UnityAction WeatherStarted;
    public event UnityAction WeatherFinished;

    [SerializeField] private AudioClip _weakRain;
    [SerializeField] private AudioClip _strongRain;
    [SerializeField] private AudioSource _audioSource;
    private Coroutine _currentSound;
    
    public void StartWeather(StrengthOfWeather strength)
    {
        _rain.Play();
        _currentSound = StartCoroutine(PlaySound(strength));
        WeatherStarted?.Invoke();
        _rain.transform.rotation = Quaternion.Euler(0,0, -Random.Range(10, 16));
    }

    private IEnumerator PlaySound(StrengthOfWeather strength)
    {
        WaitForSeconds waitForSeconds;
        AudioClip audioClip = null;
        float maxVolume = 0;
        _audioSource.volume = 0;
        switch (strength)
        {
            case StrengthOfWeather.Light:
                audioClip = _weakRain;
                maxVolume = 0.6f;
                _audioSource.clip = audioClip;
                break;
            case StrengthOfWeather.Medium:
                audioClip = _weakRain;
                maxVolume = 0.8f;
                _audioSource.clip = audioClip;
                break;
            case StrengthOfWeather.Heavy:
                audioClip = _strongRain;
                maxVolume = 0.2f;
                _audioSource.clip = audioClip;
                break;
        }
        while (true)
        {
            _audioSource.volume = 0;
            _audioSource.Play();
            waitForSeconds = new WaitForSeconds(0.02f);
            while (_audioSource.volume < maxVolume)
            {
                _audioSource.volume += maxVolume/100;
                yield return waitForSeconds;
            }

            waitForSeconds = new WaitForSeconds(audioClip.length - 4);
            yield return waitForSeconds;
                    
            waitForSeconds = new WaitForSeconds(0.02f);
            while (_audioSource.volume > 0)
            {
                _audioSource.volume -= maxVolume/100;
                yield return waitForSeconds;
            }
        }
    }

    private IEnumerator StopSound()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= 0.005f;
            yield return waitForSeconds;
        }
        _audioSource.Stop();
        StopCoroutine(_currentSound);
    }
    public void PredictNextEvent()
    {
        DateOfNextEvent = LastEventDay + Random.Range(MinDelayToNextEvent, MaxDelayToNextEvent + 1);
        HourOfNextEvent = Random.Range(1, 24); // на всякий случай от 1 до 24, а не от 0 до 24, тк как хз
        // как там просиходит событие, когда меняется день. Тонкая штука. См метод CheckDayDelayToRainfall
        _strengthOfWeather = (StrengthOfWeather)Random.Range(0, Enum.GetNames(typeof(StrengthOfWeather)).Length);
        switch (_strengthOfWeather)
        {
            case StrengthOfWeather.Light:
                SetRainParams(37, 6.5f, 0.13f);
                DurationOfEvent = Random.Range(3, 12);
                break;
            case StrengthOfWeather.Medium:
                SetRainParams(60, 7f, 0.14f);
                DurationOfEvent = Random.Range(4, 15);
                break;
            case StrengthOfWeather.Heavy:
                SetRainParams(115, 7.7f, 0.16f);
                DurationOfEvent = Random.Range(3, 9);
                break;
        }
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
    public void PrepareEvent()
    {
        AddEvent();
        PredictNextEvent();
    }
    public GlobalEvent_Weather AddEvent()
    {
        GlobalEvent_Weather eventToAdd = GlobalEventHandler.Instance
            .AddGlobalEvent<GlobalEvent_Weather>(DurationOfEvent);
        eventToAdd.StrengthOfWeather = (int)_strengthOfWeather;
        LastEventDay = GameTime.CurrentDay;
        StartWeather(_strengthOfWeather);
        return eventToAdd;
    }
    public void RemoveEvent()
    {
        _rain.Stop();
        StartCoroutine(StopSound());
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
