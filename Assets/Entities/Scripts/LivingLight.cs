using System;
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
    private float _phase;

    private void OnEnable()
    {
        GameTime.HourChanged += ChangeIntensityByTime;
        GameTime.TimeSkipped += OnTimeSkipped;
    }

    private void OnDisable()
    {
        GameTime.HourChanged -= ChangeIntensityByTime;
        GameTime.TimeSkipped -= OnTimeSkipped;
    }

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _maxIntensity = _maxBobbingIntensity;
        _minIntensity = _minBobbingIntensity;
        ChangeIntensityByTime();
        _phase = Random.Range(0, 20f);
    }

    private void Update()
    {
        float IntensityArgument = (float)Math.Sin((Time.time + _phase) / 1.8f) / 2 + 0.5f;
        _light.intensity = _maxIntensity * IntensityArgument + _minIntensity * (1 - IntensityArgument);
        float RadiusArgument = (float)Math.Sin(Time.time + _phase + 2 * IntensityArgument) / 2 + 0.5f;
        _light.pointLightOuterRadius = _maxRadius * RadiusArgument + _minRadius * (1 - RadiusArgument);
        _light.pointLightInnerRadius = _light.pointLightOuterRadius / 2.5f;
    }

    private void ChangeIntensityByTime()
    {
        switch (GameTime.Hours)
        {
            case int n when n is >= 4 and <= 10:
                _maxIntensity = Mathf.Lerp(_maxBobbingIntensity, _maxBobbingIntensity / 1.75f - 0.1f, (float)(GameTime.Hours - 3) / 7);
                _minIntensity = Mathf.Lerp(_minBobbingIntensity, _minBobbingIntensity / 1.75f - 0.1f, (float)(GameTime.Hours - 3) / 7);
                break;
            case int n when n is >= 11 and <= 14:
                _maxIntensity = _maxBobbingIntensity / 1.75f - 0.1f;
                _minIntensity = _minBobbingIntensity / 1.75f - 0.1f;
                break;
            case int n when n is >= 15 and <= 21:
                _maxIntensity = Mathf.Lerp(_maxBobbingIntensity / 1.75f - 0.1f, _maxBobbingIntensity, (float)(GameTime.Hours - 14) / 7);
                _minIntensity = Mathf.Lerp(_minBobbingIntensity / 1.75f - 0.1f, _minBobbingIntensity, (float)(GameTime.Hours - 14) / 7);
                break;
            case int n when n is >= 22 or <= 2:
                _maxIntensity = _maxBobbingIntensity;
                _minIntensity = _minBobbingIntensity;
                break;
        }
    }
    private void OnTimeSkipped(int skippedDays, int skippedHours, int skippedMinutes)
    {
        ChangeIntensityByTime();
    }
}
