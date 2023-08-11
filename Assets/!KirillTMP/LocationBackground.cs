using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationBackground : MonoBehaviour
{
    [SerializeField]private float _leftEdge;
    [SerializeField]private float _rightEdge;
    [SerializeField] private Sprite FarBg;
    [SerializeField] private Sprite NearBg;

    private void Start()
    {
        FindObjectOfType<BackgroundController>().Init(_leftEdge, _rightEdge, NearBg, FarBg);
    }
}
