using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlobalEvent
{
    public enum GlobalEventType { Weather };
    public GlobalEventType EventType { get; protected set; }

    public abstract void Execute();
    public abstract void Terminate();
}



