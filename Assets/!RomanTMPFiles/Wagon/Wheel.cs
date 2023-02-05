using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private Sprite _sprite; 
    private float _qualityModifier;
    public Sprite Sprite => _sprite;
    public float QualityModifier => _qualityModifier;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
