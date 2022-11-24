using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [SerializeField] private SlidersController _hungerScale;
    [SerializeField] private SlidersController _sleepScale;

private PlayerMover _playerMover;
    public PlayerMover PlayerMover => _playerMover;[Tooltip("—колько нужно минут, чтобы сон уменьшилс€ на 1")][SerializeField] int _sleepDecayRate;
    [Tooltip("—колько нужно минут, чтобы голод уменьшилс€ на 1")][SerializeField] int _hungerDecayRate;
    [Tooltip("—колько нужно минут во врем€ сна, чтобы сон восстановилс€ на 1")][SerializeField] int _sleepRestorationRate;    private int _currentHunger;
    private int _currentSleep;

    private int _timeCounter = 0;
    
    public int MaxHunger;
    public int MaxSleep;
    public int CurrentHunger 
    {
        get
        {
            return _currentHunger;
        }
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
        get
        {
            return _currentSleep;
        }
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

<<<<<<< .mine
    private void Start()
    {
        _playerMover = GetComponent<PlayerMover>();
    }

=======
    public bool IsSleeping;

    private void OnEnable() => GameTime.MinuteChanged += OnMinuteChange;
    private void OnDisable() => GameTime.MinuteChanged -= OnMinuteChange;

>>>>>>> .theirs
    public void RestoreHunger(int foodValue)
    {
        CurrentHunger += foodValue;
    }
    
    private void OnMinuteChange()
    {
        _timeCounter++;
        if (_timeCounter == _hungerDecayRate) 
            CurrentHunger--;
        if (!IsSleeping)
        {
            if (_timeCounter == _sleepDecayRate)
            {
                CurrentSleep--;
                _timeCounter = 0;
            }
        }
        else
        {
            if (_timeCounter == _sleepRestorationRate)
            {
                CurrentSleep++;
                _timeCounter = 0;
            }
        }
    }
}
