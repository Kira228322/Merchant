using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerWagonStats
{
    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    public float QualityModifier;

    public PlayerWagonStats(Wheel wheel, Body body, Suspension suspension)
    {
        Wheel = wheel;
        Body = body;
        Suspension = suspension;
        QualityModifier = Wheel.QualityModifier;
    }

    public event UnityAction WagonStatsRefreshed;

    public void RecalculateValues()
    {
        QualityModifier = Wheel.QualityModifier;

        int rowsToAdd = Body.InventoryRows - Player.Instance.Inventory.ItemGrid.GridSizeHeight;
        Player.Instance.Inventory.ItemGrid.AddRowsToInventory(rowsToAdd);

        Player.Instance.Inventory.MaxTotalWeight = Suspension.MaxWeight;
        WagonStatsRefreshed?.Invoke();
    }
}
