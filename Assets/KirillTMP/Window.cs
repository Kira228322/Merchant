using System;
using UnityEngine;

public abstract class Window : MonoBehaviour
{
    private void Start()
    {
        MapManager.Windows.Add(this);
    }

    private void OnDestroy()
    {
        MapManager.Windows.Remove(this);
    }
}
