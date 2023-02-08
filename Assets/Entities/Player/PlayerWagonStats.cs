using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        int rowsToAdd = Body.InventoryRows - Player.Singleton.Inventory.ItemGrid.GridSizeHeight;
        Player.Singleton.Inventory.ItemGrid.AddRowsToInventory(rowsToAdd);

        Player.Singleton.Inventory.MaxTotalWeight = Suspension.MaxWeight;
        WagonStatsRefreshed?.Invoke();
    }
}
