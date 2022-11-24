using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private SlidersController _hungerScale;
    [SerializeField] private SlidersController _sleepScale;

    private int _currentHunger;
    private int _currentSleep;
    
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

    public void RestoreHunger(int foodValue)
    {
        CurrentHunger += foodValue;
    }

    public void Sleep()
    {
        // TODO думаю сделать так, чтобы было можно было выбирать время сна и за каждый час сна восстанавливать
        // 1/8 от MaxSleep (то есть максимум можно поспать 8 часов, больше нет смысла) 
    }
    
}
