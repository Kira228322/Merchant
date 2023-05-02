using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalEventHandlerSaveData
{
    public List<GlobalEvent_Base> SavedGlobalEvents;
    public GlobalEventHandlerSaveData(List<GlobalEvent_Base> activeGlobalEvents)
    {
        SavedGlobalEvents = new(activeGlobalEvents);
    }
}
