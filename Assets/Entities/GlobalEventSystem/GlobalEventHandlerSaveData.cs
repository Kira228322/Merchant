using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEventHandlerSaveData
{
    public List<GlobalEvent_Base> SavedGlobalEvents;
    public GlobalEventHandlerSaveData(List<GlobalEvent_Base> activeGlobalEvents)
    {
        SavedGlobalEvents = new(activeGlobalEvents);
    }
}
