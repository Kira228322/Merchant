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
    public string ItemToMultiplyName => "Крутецкий кактус"; //TODO: пока пшеницы нет, добавляю Крутецкий кактус.

    public override void Execute()
    {
        Location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
        //TODO когда будет категория сельхозпродукта в датабазе:
        //ItemDatabase.GetRandomItemOfThisType(Item.ItemType.GrownFood)
    }

    public override void Terminate()
    {
        //Одноразовый ивент.
        //После высокого прироста пшеницы она медленно начнет возвращаться сама из-за системы экономики.
        //Поэтому ничего не происходит когда ивент заканчивается.
    }
}
