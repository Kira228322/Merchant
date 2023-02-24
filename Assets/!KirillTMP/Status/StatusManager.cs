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
        for (int i = 0; i < _container.childCount; i++)
        {
            if (_container.GetChild(i).GetComponent<StatusUIObject>().Status == status)
            {
                _container.GetChild(i).GetComponent<StatusUIObject>().RefreshStatus();
                return;
            }
        }
        
        Instantiate(_statusPrefub, _container).GetComponent<StatusUIObject>().Init(status);
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }
}
