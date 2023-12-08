using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEventHandlerSaveData
{
    public List<GlobalEvent_Base> SavedGlobalEvents;
    public List<EventControllerSaveData> EventControllerSaveDatas;
    public GlobalEventHandlerSaveData(List<GlobalEvent_Base> activeGlobalEvents, List<IEventController> eventControllers)
    {
        SavedGlobalEvents = new(activeGlobalEvents);
        EventControllerSaveDatas = new();
        foreach (var controller in eventControllers)
        {
            EventControllerSaveDatas.Add(new(controller.LastEventDay));
        }
    }

}
