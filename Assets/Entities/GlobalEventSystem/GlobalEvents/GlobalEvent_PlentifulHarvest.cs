using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_PlentifulHarvest : GlobalEvent_Base
{
    public override string GlobalEventName => $"Сверхурожай в {Location.VillageName}!";

    public override string Description => $"Благодаря стараниям рабочих, в {Location.VillageName} ожидается удивительно высокий урожай.";


    [NonSerialized] public Location Location;
    public float MultiplyCoefficient;
    public string ItemToMultiplyName => "Крутецкий кактус"; //TODO: убрать, когда будет определение айтема в контроллере.

    public override void Execute()
    {
        Location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
    }

    public override void Terminate()
    {
        //Одноразовый ивент.
        //После высокого прироста еды она медленно начнет возвращаться сама из-за системы экономики.
        //Поэтому ничего не происходит когда ивент заканчивается.
    }
}
