using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Test_GlobalEvent : IGlobalEvent
{
    public static bool IsConcurrent { get; } = true;
    public int DurationHours { get; set; }
    public void Execute()
    {
        Debug.Log("Executed TestGlobalEvent");
    }

    public void Terminate()
    {
        Debug.Log("Terminated TestGlobalEvent");
    }
}
