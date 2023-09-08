using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerNeeds: ISaveable<PlayerNeedsSaveData>
{
    [SerializeField] private SlidersController _hungerScale;
    [SerializeField] private SlidersController _sleepScale;

    [SerializeField][HideInInspector] public int HungerDecayRate; //"—колько нужно минут, чтобы голод уменьшилс€ на 1"
    [SerializeField][HideInInspector] public int HungerDivisorWhenSleeping = 2; // "¬о сколько раз замедл€етс€ падение голода во врем€ сна"
    [SerializeField][HideInInspector] public int SleepDecayRate; //"—колько нужно минут, чтобы сон уменьшилс€ на 1"
    [SerializeField][HideInInspector] public int SleepRestorationRate = 5; // "—колько нужно минут во врем€ сна, чтобы сон восстановилс€ на 1"

    [SerializeField][HideInInspector] private int _currentHunger;
    [SerializeField][HideInInspector] private int _currentSleep;

    private int _generalTimeCounter = 0;
    private int _timeCounterWhenSleeping = 0;
    private int _hoursLeftToSleep = 0;

    [HideInInspector] public int MaxHunger;
    [HideInInspector] public int MaxSleep = 90;
    public int CurrentHunger
    {
        get => _currentHunger;
        set
        {
            _currentHunger = value;
            if (_currentHunger < 0)
            {
                _currentHunger = 0;
            }
            if (_currentHunger > MaxHunger)
            {
                _currentHunger = MaxHunger;
            }
            _hungerScale.SetValue(CurrentHunger, MaxHunger);
        }
    }
    public int CurrentSleep
    {
        get => _currentSleep;
        set
        {
            _currentSleep = value;
            if (_currentSleep < 0)
            {
                _currentSleep = 0;
            }
            if (_currentSleep > MaxSleep)
            {
                _currentSleep = MaxSleep;
            }
            _sleepScale.SetValue(CurrentSleep, MaxSleep);
        }
    }
    public bool IsSleeping;

    public event UnityAction SleptOneHourEvent;
    public event UnityAction FinishedSleeping;

    public void Initialize()
    {
        _sleepScale.SetValue(CurrentSleep, MaxSleep);
        _hungerScale.SetValue(CurrentHunger, MaxHunger);
    }

    public void StartSleeping(int hours)
    {
        HungerDecayRate *= HungerDivisorWhenSleeping; //голод замедл€етс€ в _hungerMultiplierWhenSleeping раз
        _hoursLeftToSleep = hours;
        IsSleeping = true;
    }
    public void StopSleeping()
    {
        HungerDecayRate /= HungerDivisorWhenSleeping; //голод возвращаетс€ в предыдущую скорость падени€
        IsSleeping = false;
        FinishedSleeping?.Invoke();
    }
    public void RestoreHunger(int foodValue)
    {
        CurrentHunger += foodValue;
    }

    public void RestoreSleep(int value)
    {
        CurrentSleep += value;
    }
    private void SleptOneHour()
    {
        _hoursLeftToSleep--;

        if (_hoursLeftToSleep <= 0)
        {
            StopSleeping();
        }
        SleptOneHourEvent?.Invoke();
    }
    public void UpdateNeeds()
    {
        _generalTimeCounter++;
        if (_currentHunger <= 0 || _currentSleep <= 0)
            StatusManager.Instance.AddLowNeedsDebuff();
        
        if (_generalTimeCounter % HungerDecayRate == 0)
        {
            CurrentHunger--;
        }
        if (IsSleeping)
        {
            _timeCounterWhenSleeping++;
            if (_timeCounterWhenSleeping % SleepRestorationRate == 0)
            {
                CurrentSleep++;
            }
            if (_timeCounterWhenSleeping == 60)
            {
                _timeCounterWhenSleeping = 0;
                SleptOneHour();
            }
        }
        else
        {
            if (_generalTimeCounter % SleepDecayRate == 0)
            {
                CurrentSleep--;
            }
        }
    }

    public PlayerNeedsSaveData SaveData()
    {
        PlayerNeedsSaveData saveData = new(this);
        return saveData;
    }

    public void LoadData(PlayerNeedsSaveData saveData)
    {
        CurrentHunger = saveData.currentHunger;
        CurrentSleep = saveData.currentSleep;

        MaxHunger = saveData.maxHunger;
        MaxSleep = saveData.maxSleep;
    }
}
