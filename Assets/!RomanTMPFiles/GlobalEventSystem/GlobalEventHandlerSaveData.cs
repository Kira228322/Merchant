using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalEventHandlerSaveData
{
    public List<IGlobalEvent> SavedGlobalEvents;

    public GlobalEventHandlerSaveData(List<IGlobalEvent> activeGlobalEvents)
    {
        SavedGlobalEvents = new(activeGlobalEvents);
    }
}
