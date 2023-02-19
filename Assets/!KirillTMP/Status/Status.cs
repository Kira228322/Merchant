using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    [SerializeField] private Image _icon;
    private void OnEnable()
    {
        Activation();
    }

    private void OnDisable()
    {
        Deactivation(); 
    }

    private void Activation()
    {
        
    }

    private void Deactivation()
    {
        
    }
}
