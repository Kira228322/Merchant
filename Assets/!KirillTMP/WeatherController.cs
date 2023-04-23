using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class WeatherController : MonoBehaviour
{

    //(21.04.23) Был проведен реворк данного скрипта с целью привязать его к системе ивентов
    //Изменился принцип действия, теперь будет так: this считает, когда пора начинать дождь, как и прежде. Когда это время наступает,
    //он сигнализирует в GlobalEventHandler, что надо создать GlobalEvent_Weather, передаёт туда информацию
    //(информация - сила погоды и её длительность). После создания ивента, он (ивент) запускает StartWeather() в this.
    //После этого время считать будет уже GlobalEventHandler. Когда пройдет нужное время,
    //то GlobalEventHandler остановит свой ивент, а ивент остановит партиклы у this, параллельно запуская PredictNextWeather.
    //Вот такая немного запутанная структура, тем не менее объединяющая системы погоды и ивентов.
    //
    //  P.S. Сразу отвечу на вопрос "нахуя, а главное зачем?" - так можно будет легко получать информацию о том,
    //идёт ли сейчас дождь и знать его силу. Может использоваться, например, в диалогах или в каких-то других условиях
    //  P.P.S. Ещё переименовал почти все упоминания Rain -> Weather. Ну, я полагаю, как минимум снег тоже будет.
    //Там просто другую партиклсистему добавим и сделаем енум в будущем.

    [SerializeField] private ParticleSystem _rain;
    private enum StrengthOfWeather {Light, Medium, Heavy}
    private StrengthOfWeather _strengthOfWeather;
    
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
        // TODO потом можно еще менять глобальное освещение
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
        if (_dateOfPrecipitation == GameTime.CurrentDay)
        {
            GameTime.HourChanged += CheckHourDelayToPrecipitation;
            GameTime.DayChanged -= CheckDayDelayToPrecipitation;
        }
    }

    private void CheckHourDelayToPrecipitation()
    {
        if (_hourOfPrecipitation == GameTime.Hours)
        {
            GameTime.HourChanged -= CheckHourDelayToPrecipitation;

            GlobalEvent_Weather eventToAdd = (GlobalEvent_Weather)GlobalEventHandler.Instance.AddGlobalEvent
                (typeof(GlobalEvent_Weather), _durationOfWeatherInHours);
            eventToAdd.StrengthOfWeather = (int)_strengthOfWeather;
        }
    }

    private void PredictNextPrecipitation()
    {
        _dateOfPrecipitation = GameTime.CurrentDay + Random.Range(_minDelayToNextPrecipitation, _maxDelayToNextPrecipitation + 1);
        _hourOfPrecipitation = Random.Range(1, 24); // на всякий случай от 1 до 24, а не от 0 до 24, тк как хз
        // как там просиходит событие, когда меняется день. Тонкая штука. См метод CheckDayDelayToRainfall
        
        _strengthOfWeather = (StrengthOfWeather)Random.Range(0, Enum.GetNames(typeof(StrengthOfWeather)).Length);
        switch (_strengthOfWeather)
        {
            case StrengthOfWeather.Light:
                _durationOfWeatherInHours = Random.Range(3, 12);
                break;
            case StrengthOfWeather.Medium:
                _durationOfWeatherInHours = Random.Range(4, 15);
                break;
            case StrengthOfWeather.Heavy:
                _durationOfWeatherInHours = Random.Range(3, 8);
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
}
