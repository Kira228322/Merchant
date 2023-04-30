using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalEventHandlerSaveData
{
    public List<IGlobalEvent> SavedGlobalEvents;
    public List<GlobalEventHandler.EventTypeInformation> CooldownInformations;
    public GlobalEventHandlerSaveData(List<IGlobalEvent> activeGlobalEvents, 
        List<GlobalEventHandler.EventTypeInformation> cooldownInformations)
    {
        SavedGlobalEvents = new(activeGlobalEvents);
        CooldownInformations = new(cooldownInformations);
    }
}
