using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Light2D))]
public class LivingLight : MonoBehaviour
{
    
    private Light2D _light;
    [SerializeField] private float _minBobbingIntensity;
    [SerializeField] private float _maxBobbingIntensity;
    [SerializeField] private float _maxRadius;
    [SerializeField] private float _minRadius;
    private float _maxIntensity;
    private float _minIntensity;

    private void OnEnable()
    {
        GameTime.HourChanged += ChangeIntensityByTime;
    }

    private void OnDisable()
    {
        GameTime.HourChanged -= ChangeIntensityByTime;
    }

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _maxIntensity = _maxBobbingIntensity;
        _minIntensity = _minBobbingIntensity;
        ChangeIntensityByTime();
    }

    private void Update()
    {
        float IntensityArgument = (float)Math.Sin(Time.time / 1.8f)/2 + 0.5f ;
        _light.intensity = _maxIntensity * IntensityArgument + _minIntensity * (1-IntensityArgument);
        float RadiusArgument = (float)Math.Sin(Time.time + 2 * IntensityArgument)/2 + 0.5f;
        _light.pointLightOuterRadius = _maxRadius * RadiusArgument + _minRadius * (1-RadiusArgument);
        _light.pointLightInnerRadius = _light.pointLightOuterRadius/2.75f;
    }

    private void ChangeIntensityByTime()
    {
        switch (GameTime.Hours)
        {
            case int n when n >= 3 && n <= 10:
                _maxIntensity = Mathf.Lerp(_maxBobbingIntensity, _maxBobbingIntensity/1.8f -0.2f, (float)(GameTime.Hours-2)/8);
                _minIntensity = Mathf.Lerp(_minBobbingIntensity, _minBobbingIntensity/1.8f -0.2f, (float)(GameTime.Hours-2)/8);
                break;
            case int n when n >= 11 && n <= 14:
                _maxIntensity = _maxBobbingIntensity / 1.8f - 0.2f;
                _minIntensity = _minBobbingIntensity / 1.8f - 0.2f;
                break;
            case int n when n >= 15 && n <= 22:
                _maxIntensity = Mathf.Lerp(_maxBobbingIntensity/1.8f -0.2f, _maxBobbingIntensity, (float)(GameTime.Hours-14)/8);
                _minIntensity = Mathf.Lerp(_minBobbingIntensity/1.8f -0.2f, _minBobbingIntensity, (float)(GameTime.Hours-14)/8);
                break;
            case int n when n >= 23 || n <= 2:
                _maxIntensity = _maxBobbingIntensity;
                _minIntensity = _minBobbingIntensity;
                break;
        }
    }
}
