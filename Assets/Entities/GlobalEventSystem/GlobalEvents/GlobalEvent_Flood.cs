using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_Flood : GlobalEvent_Base
{
    public override string GlobalEventName => $"Наводнение в {Location.VillageName}!";

    public override string Description => $"В {Location.VillageName} произошло наводнение. Ожидается ухудшение урожая пшеницы.";

    public Location Location;
    public string ItemToDeleteName => "Крутецкий кактус"; //TODO: пока пшеницы нет, удаляю Крутецкий кактус.
    public override void Execute()
    {
        Location.DeleteItemsFromTraders(ItemToDeleteName);
      //TODO когда будет категория сельхозпродукта в датабазе:
      //ItemDatabase.GetRandomItemOfThisType(Item.ItemType.GrownFood)
    }

    public override void Terminate()
    {
        //Одноразовый ивент.
        //После удаления пшеницы она медленно начнет возвращаться сама из-за системы экономики.
        //Поэтому ничего не происходит когда ивент заканчивается.
    }
}
