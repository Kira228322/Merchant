using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasWarningGenerator : MonoBehaviour
{
    [SerializeField] private WarningWindow _warningWindowPrefub;

    public void CreateWarning(string label, string massage)
    {
        Instantiate(_warningWindowPrefub.gameObject, transform).GetComponent<WarningWindow>().Init(label, massage);
    }
}
