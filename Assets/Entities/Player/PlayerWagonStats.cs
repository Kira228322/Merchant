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

    public event UnityAction WagonStatsRefreshed; 

    public void Initialize(Wheel wheel, Body body, Suspension suspension)
    {
        Wheel = wheel;
        Body = body;
        Suspension = suspension;
        QualityModifier = Wheel.QualityModifier;
    }

    public void RecalculateValues()
    {
        QualityModifier = Wheel.QualityModifier;

        int rowsToAdd = Body.InventoryRows - Player.Singleton.Inventory.ItemGrid.GridSizeHeight;
        Player.Singleton.Inventory.ItemGrid.AddRowsToInventory(rowsToAdd);

        Player.Singleton.Inventory.ItemGrid.MaxTotalWeight = Suspension.MaxWeight;
        WagonStatsRefreshed?.Invoke();
    }
}
