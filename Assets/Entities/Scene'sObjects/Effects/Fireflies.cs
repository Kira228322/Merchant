using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fireflies : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private int _hourStart;
    private int _hourStop;

    private void OnEnable()
    {
        _hourStart = Random.Range(20, 22);
        _hourStop = Random.Range(5, 7);
        GameTime.HourChanged += OnHourChangeWhenDisable;
        OnHourChangeWhenDisable();
    }

    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChangeWhenDisable;
        GameTime.HourChanged -= OnHourChangeWhenEnable;
    }

    private void OnHourChangeWhenDisable()
    {
        if (GameTime.Hours >= _hourStart || GameTime.Hours < _hourStop)
        {
            _particleSystem.Play();
            GameTime.HourChanged -= OnHourChangeWhenDisable;
            GameTime.HourChanged += OnHourChangeWhenEnable;
        }
    }

    private void OnHourChangeWhenEnable()
    {
        if (GameTime.Hours >= _hourStop && GameTime.Hours < _hourStart)
        {
            _particleSystem.Stop();
            GameTime.HourChanged -= OnHourChangeWhenEnable;
            GameTime.HourChanged += OnHourChangeWhenDisable;
        }
    }
}
