using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TravelTimeCounter : MonoBehaviour
{
    // Вынес отсчет времени во время сцены в отдельный скрипт, что бы не проверять кучу проверок (см update этого скрипта)
    // каждый фрейм постоянно, а проверять лишь тогда, когда это надо (во время поездки) 
    [SerializeField] private TMP_Text _travelTime;
    private int _duration;
    private float _minutes;
    
    public void Init(int duration)
    {
        _duration = duration;
        _minutes = 0;
        enabled = true;
    }

    private void Update() 
    {
        _minutes += Time.deltaTime * GameTime.GetTimeScale();
        if (_minutes >= 60)
        {
            _minutes -= 60;
            _duration--;
            if (_duration / 24 == 0)
                _travelTime.text = _duration + " часов";
            else
                _travelTime.text = _duration/24 + " дней " + _duration % 24 + " часов";
            if (_duration == 0)
            {
                // TODO Переход на сцену, в которую игрок ехал
            }
        }
    }
}
