using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerWagonStats: ISaveable<PlayerWagonStatsSaveData>
{
    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    [HideInInspector] public float QualityModifier;

    public event UnityAction WagonStatsRefreshed;

    public void UpgradeWagonPart(WagonPart wagonPart)
    {
        if (wagonPart is Wheel)
            Wheel = (Wheel)wagonPart;
        if (wagonPart is Body)
            Body = (Body)wagonPart;
        if (wagonPart is Suspension)
            Suspension = (Suspension)wagonPart;
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
