using System;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

[Serializable]
public class GlobalEventHandlerSaveData
{
    public List<GlobalEvent_BanLift> SavedBanLifts;
    public List<GlobalEvent_Flood> SavedFloods;
    public List<GlobalEvent_Weather> SavedWeathers;
    public List<GlobalEvent_DangerousRoad> SavedRoads;
    public List<GlobalEvent_BannedItem> SavedBannedItems;
    public List<GlobalEvent_MultiplyItemsOnScene> SavedMultiplyItemsOnScenes;
    public List<EventControllerSaveData> EventControllerSaveDatas;
    public GlobalEventHandlerSaveData(List<GlobalEvent_Base> activeGlobalEvents, List<IEventController> eventControllers)
    {
        SavedBanLifts = new List<GlobalEvent_BanLift>();
        SavedFloods = new List<GlobalEvent_Flood>();
        SavedWeathers = new List<GlobalEvent_Weather>();
        SavedRoads = new List<GlobalEvent_DangerousRoad>();
        SavedBannedItems = new List<GlobalEvent_BannedItem>();
        SavedMultiplyItemsOnScenes = new List<GlobalEvent_MultiplyItemsOnScene>();
        foreach (var globalEvent in activeGlobalEvents)
        {
            switch (globalEvent)
            {
                case { } x when x is GlobalEvent_BanLift:
                    SavedBanLifts.Add((GlobalEvent_BanLift)globalEvent);
                    break;
                case { } x when x is GlobalEvent_Flood:
                    SavedFloods.Add((GlobalEvent_Flood)globalEvent);
                    break;
                case { } x when x is GlobalEvent_BannedItem:
                    SavedBannedItems.Add((GlobalEvent_BannedItem)globalEvent);
                    break;
                case { } x when x is GlobalEvent_Weather:
                    SavedWeathers.Add((GlobalEvent_Weather)globalEvent);
                    break;
                case { } x when x is GlobalEvent_DangerousRoad:
                    SavedRoads.Add((GlobalEvent_DangerousRoad)globalEvent);
                    break;
                case { } x when x is GlobalEvent_MultiplyItemsOnScene:
                    SavedMultiplyItemsOnScenes.Add((GlobalEvent_MultiplyItemsOnScene)globalEvent);
                    break;
            }
        }

        
        EventControllerSaveDatas = new();
        foreach (var controller in eventControllers)
        {
            EventControllerSaveDatas.Add(new(controller.LastEventDay));
        }
    }

}
