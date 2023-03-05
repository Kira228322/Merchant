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


    public void RecalculateValues()
    {
        QualityModifier = Wheel.QualityModifier;

        int rowsToAdd = Body.InventoryRows - Player.Instance.Inventory.ItemGrid.GridSizeHeight;
        Player.Instance.Inventory.ItemGrid.AddRowsToInventory(rowsToAdd);

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
