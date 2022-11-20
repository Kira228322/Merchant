using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{
    [SerializeField] private Image _fillArea;
    [SerializeField] private Gradient _colour;
    private Slider _slider; 
    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetValue(float value, float maxValue)
    {
        _slider.value = value / maxValue;
        if (_colour.Evaluate(1) != Color.white)
            _fillArea.color = _colour.Evaluate(_slider.normalizedValue);
        
    }

    public void SetColour(Color color)
    {
        _fillArea.color = color;
    }
}
