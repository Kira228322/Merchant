using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_MultiplyItemsOnScene : GlobalEvent_Base
{
    public override string GlobalEventName => IsPositive? ($"¬ысокий прирост продуктов в {Location.VillageName}!")
                                                         :($"”быток продукта в {Location.VillageName}");

    public override string Description => IsPositive? ($"Ѕлагодар€ старани€м рабочих, в {Location.VillageName} ожидаетс€ удивительно высокий прирост продукта {ItemToMultiplyName}.")
                                                     :($"»з-за негативного вли€ни€ магии на деревню {Location.VillageName}, в ней ожидаетс€ убыток продукта {ItemToMultiplyName}");

    public bool IsPositive;
    [NonSerialized] public Location Location;
    public float MultiplyCoefficient;
    public string ItemToMultiplyName;

    public override void Execute()
    {
        Location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
    }

    public override void Terminate()
    {
        //ќдноразовый ивент.
        //ѕосле высокого/низкого прироста еды она медленно начнет возвращатьс€ сама из-за системы экономики.
        //ѕоэтому ничего не происходит когда ивент заканчиваетс€.
    }
}
