using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{
    [SerializeField] private Image _fillArea;
    private Slider _slider; 
    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetValue(float value, float maxValue)
    {
        _slider.value = value / maxValue;
    }

    public void SetColour(Color color)
    {
        _fillArea.color = color;
    }
}
