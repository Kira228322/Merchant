using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : WagonPart
{
    [SerializeField] private float _qualityModifier;
    public float QualityModifier => _qualityModifier;
    public override void Replace(WagonPart wagonPart)
    {
        base.Replace(wagonPart);
        Wheel wheel = wagonPart as Wheel;
        _qualityModifier = wheel.QualityModifier;
    }
}
