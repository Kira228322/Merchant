using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalEvent
{
    static bool IsConcurrent { get; }
    int DurationHours { get; set; }
    void Execute();
    void Terminate();
}