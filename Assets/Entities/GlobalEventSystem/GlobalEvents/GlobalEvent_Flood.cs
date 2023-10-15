using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_Flood : GlobalEvent_Base
{
    public override string GlobalEventName => $"Наводнение в деревне {Location.VillageName}!";
    public override string Description => $"В деревне {Location.VillageName} произошло наводнение. Больше всего пострадали запасы продукта {ItemToMultiplyName}.";

    [NonSerialized] public Location Location;
    public float MultiplyCoefficient;
    public string ItemToMultiplyName;

    public override void Execute()
    {
        Location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
    }

    public override void Terminate()
    {
        //Одноразовый ивент.
        //После высокого/низкого прироста еды она медленно начнет возвращаться сама из-за системы экономики.
        //Поэтому ничего не происходит когда ивент заканчивается.
    }
}
