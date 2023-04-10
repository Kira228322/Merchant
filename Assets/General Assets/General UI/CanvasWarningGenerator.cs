using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasWarningGenerator : MonoBehaviour
{
    [SerializeField] private WarningWindow _warningWindowPrefab;

    public static CanvasWarningGenerator Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CreateWarning(string label, string message)
    {
        Instantiate(_warningWindowPrefab.gameObject, transform).GetComponent<WarningWindow>().Init(label, message);
    }
}
