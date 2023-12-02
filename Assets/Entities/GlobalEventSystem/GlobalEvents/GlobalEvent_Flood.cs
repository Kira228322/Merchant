using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_Flood : GlobalEvent_Base
{
    public override string GlobalEventName => $"Наводнение в деревне {LocationVillageName}!";
    public override string Description => $"В деревне {LocationVillageName} произошло наводнение. Больше всего пострадали запасы продукта {ItemToMultiplyName}.";

    public string LocationSceneName;
    public string LocationVillageName;
    public float MultiplyCoefficient;
    public string ItemToMultiplyName;

    public override void Execute()
    {
        Location location = MapManager.GetLocationBySceneName(LocationSceneName);
        location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
    }

    public override void Terminate()
    {
        //Одноразовый ивент.
        //После высокого/низкого прироста еды она медленно начнет возвращаться сама из-за системы экономики.
        //Поэтому ничего не происходит когда ивент заканчивается.
    }
}
