using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeflow : MonoBehaviour
{
    public float TimeScale; // TimeScale должен влиять на скорость времени суток и на скорость порчи продуктов

    private float _timeCounter;
    private void Update()
    {
        _timeCounter += Time.deltaTime * TimeScale;
        if (_timeCounter >= 1f) 
        { 
            GameTime.Minutes++;
            _timeCounter = 0;
        }
    }
}
