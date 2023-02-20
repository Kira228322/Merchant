using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [HideInInspector] public static StatusManager Instance;
    
    [SerializeField] private GameObject _statusPrefub;
    [SerializeField] private Transform _container;
    
    public void AddStatusForPlayer(Status status)
    {
        Instantiate(_statusPrefub, _container).GetComponent<StatusUIObject>().Init(status);
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }
}
