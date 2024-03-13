using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerWagonStats : ISaveable<PlayerWagonStatsSaveData>
{
    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    [HideInInspector] public float QualityModifier;

    public event UnityAction WagonStatsRefreshed;

    public void UpgradeWagonPart(WagonPart wagonPart)
    {
        if (wagonPart is Wheel wheel)
            Wheel = wheel;
        if (wagonPart is Body body)
            Body = body;
        if (wagonPart is Suspension suspension)
            Suspension = suspension;
        RecalculateValues();
    }

    public void RecalculateValues()
    {
        QualityModifier = Wheel.QualityModifier;

        int rowsToAdd = Body.InventoryRows - Player.Instance.Inventory.BaseItemGrid.GridSizeHeight;
        Player.Instance.Inventory.BaseItemGrid.AddRowsToInventory(rowsToAdd);

        Player.Instance.Inventory.MaxTotalWeight = Suspension.MaxWeight;
        WagonStatsRefreshed?.Invoke();
    }

    public PlayerWagonStatsSaveData SaveData()
    {
        PlayerWagonStatsSaveData saveData = new(this);
        return saveData;
    }
    public void LoadData(PlayerWagonStatsSaveData data)
    {
        Suspension = (Suspension)WagonPartDatabase.GetWagonPart(data.SuspensionName);
        Wheel = (Wheel)WagonPartDatabase.GetWagonPart(data.WheelName);
        Body = (Body)WagonPartDatabase.GetWagonPart(data.BodyName);

        RecalculateValues();
    }
}
