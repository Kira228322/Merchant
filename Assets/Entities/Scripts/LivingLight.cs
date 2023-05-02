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
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;
    [SerializeField] private float _maxRadius;
    [SerializeField] private float _minRadius;
    private void Start()
    {
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        float IntensityArgument = (float)Math.Sin(Time.time / 1.8f)/2 + 0.5f ;
        _light.intensity = _maxIntensity * IntensityArgument + _minIntensity * (1-IntensityArgument);
        float RadiusArgument = (float)Math.Sin(Time.time + 2 * IntensityArgument)/2 + 0.5f;
        _light.pointLightOuterRadius = _maxRadius * RadiusArgument + _minRadius * (1-RadiusArgument);
        _light.pointLightInnerRadius = _light.pointLightOuterRadius/2.75f;
    }
}
