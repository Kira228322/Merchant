using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasWarningGenerator : MonoBehaviour
{
    [SerializeField] private WarningWindow _warningWindowPrefub;

    public static CanvasWarningGenerator Instance;

    private void Start()
    {
        if (Instance != null) 
            Debug.Log("Warning generator уже существует");
        Instance = this;
    }

    public void CreateWarning(string label, string message)
    {
        Instantiate(_warningWindowPrefub.gameObject, transform).GetComponent<WarningWindow>().Init(label, message);
    }
}
