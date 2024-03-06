using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WeatherController : MonoBehaviour, IEventController<GlobalEvent_Weather>, ISaveable<EventControllerSaveData>
{
    [SerializeField] private ParticleSystem _rain;
    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private WindSound _windSound;
    public enum StrengthOfWeather { Light, Medium, Heavy }
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

    public void StartWeather()
    {
        switch (_strengthOfWeather)
        {
            case StrengthOfWeather.Light:
                SetRainParams(32, 6.5f, 0.14f);
                break;
            case StrengthOfWeather.Medium:
                SetRainParams(55, 7f, 0.15f);
                break;
            case StrengthOfWeather.Heavy:
                SetRainParams(110, 7.5f, 0.17f);
                break;
        }
        _rain.Play();
        _currentSound = StartCoroutine(PlaySound(_strengthOfWeather));
        WeatherStarted?.Invoke();
        _rain.transform.rotation = Quaternion.Euler(0, 0, -Random.Range(10, 16));
    }

    private IEnumerator PlaySound(StrengthOfWeather strength)
    {
        _audioMixer.audioMixer.GetFloat("MusicParentVolume", out var MusicParentVolume);
        _audioMixer.audioMixer.SetFloat("MusicParentVolume", MusicParentVolume - 2.25f);
        WaitForSeconds waitForSeconds;
        AudioClip audioClip = null;
        float maxVolume = 0;
        _audioSource.volume = 0;
        switch (strength)
        {
            case StrengthOfWeather.Light:
                audioClip = _weakRain;
                maxVolume = 0.62f;
                _windSound.Volume = 0.77f;
                break;
            case StrengthOfWeather.Medium:
                audioClip = _weakRain;
                maxVolume = 0.8f;
                _windSound.Volume = 0.82f;
                break;
            case StrengthOfWeather.Heavy:
                audioClip = _strongRain;
                maxVolume = 0.09f;
                _windSound.Volume = 0.87f;
                break;
        }
        _audioSource.clip = audioClip;
        _audioSource.volume = 0;
        _audioSource.Play();
        waitForSeconds = new WaitForSeconds(0.02f);
        for (int i = 0; i < 100; i++)
        {
            _audioSource.volume += maxVolume / 100;
            yield return waitForSeconds;
        }

        _currentSound = null;
    }

    private IEnumerator StopSound()
    {
        _windSound.Volume = 0.67f;
        _audioMixer.audioMixer.GetFloat("MusicParentVolume", out var MusicParentVolume);
        _audioMixer.audioMixer.SetFloat("MusicParentVolume", MusicParentVolume + 2.25f);
        WaitForSeconds waitForSeconds = new(0.02f);
        float maxVolume = _audioSource.volume;
        for (int i = 0; i < 50; i++)
        {
            _audioSource.volume -= maxVolume / 50;
            yield return waitForSeconds;
        }
        _audioSource.Stop();
        if (_currentSound != null)
            StopCoroutine(_currentSound);
    }
    public void PredictNextEvent()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            DateOfNextEvent = LastEventDay + 2;
            HourOfNextEvent = Random.Range(1, 24);
        }
        else
        {
            DateOfNextEvent = LastEventDay + Random.Range(MinDelayToNextEvent, MaxDelayToNextEvent + 1);
            HourOfNextEvent = Random.Range(1, 24); // на всякий случай от 1 до 24, а не от 0 до 24, тк как хз
                                                   // как там просиходит событие, когда меняется день. Тонкая штука. См метод CheckDayDelayToRainfall
        }
        _strengthOfWeather = (StrengthOfWeather)Random.Range(0, Enum.GetNames(typeof(StrengthOfWeather)).Length);
        switch (_strengthOfWeather)
        {
            case StrengthOfWeather.Light:
                DurationOfEvent = Random.Range(3, 12);
                break;
            case StrengthOfWeather.Medium:
                DurationOfEvent = Random.Range(4, 15);
                break;
            case StrengthOfWeather.Heavy:
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
        //StartWeather(); вынесено в глобальный ивент
        return eventToAdd;
    }
    public void RemoveEvent()
    {
        _rain.Stop();
        StartCoroutine(StopSound());
        WeatherFinished?.Invoke();
    }

    public EventControllerSaveData SaveData()
    {
        EventControllerSaveData saveData = new(LastEventDay);
        return saveData;
    }

    public void LoadData(EventControllerSaveData data)
    {
        LastEventDay = data.LastEventDay;
    }
}
