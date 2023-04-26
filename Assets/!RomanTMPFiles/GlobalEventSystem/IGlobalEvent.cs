using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalEvent
{
    static bool IsConcurrent { get; } //Может ли одновременно быть два и более ивента такого типа?
    int DurationHours { get; set; }
    void Execute();
    void Terminate();
}