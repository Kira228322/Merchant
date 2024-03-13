using System;

[Serializable]
public class GlobalEvent_MultiplyItemsOnScene : GlobalEvent_Base
{
    public override string GlobalEventName => IsPositive ? ($"¬ысокий прирост продуктов в {LocationVillageName}!")
                                                         : ($"”быток продукта в {LocationVillageName}");

    public override string Description => IsPositive ? ($"Ѕлагодар€ старани€м рабочих, в {LocationVillageName} ожидаетс€ удивительно высокий прирост продукта {ItemToMultiplyName}.")
                                                     : ($"»з-за негативного вли€ни€ магии на деревню {LocationVillageName}, в ней ожидаетс€ убыток продукта {ItemToMultiplyName}");

    public bool IsPositive;
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
        //ќдноразовый ивент.
        //ѕосле высокого/низкого прироста еды она медленно начнет возвращатьс€ сама из-за системы экономики.
        //ѕоэтому ничего не происходит когда ивент заканчиваетс€.
    }
}
