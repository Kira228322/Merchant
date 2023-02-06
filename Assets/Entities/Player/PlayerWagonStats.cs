using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWagonStats: MonoBehaviour
{
    [SerializeField] private Wheel _startingWheel;
    [SerializeField] private Body _startingBody;
    [SerializeField] private Suspension _startingSuspension;

    private float _qualityModifier;

    public float QualityModifier => _qualityModifier;

    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    public event UnityAction WagonStatsRefreshed; 

    private void Start()
    {
        Wheel = _startingWheel;
        Body = _startingBody;
        Suspension = _startingSuspension;

        _qualityModifier = Wheel.QualityModifier;
        //Body value? Suspension value?
    }

    public void RefreshStats(Wheel wheel, Body body, Suspension suspension)
    {
        Wheel = wheel;
        Body = body;
        Suspension = suspension;
        RecalculateValues();
        WagonStatsRefreshed?.Invoke();
    }
    public void RecalculateValues()
    {
        _qualityModifier = Wheel.QualityModifier;

        int rowsToAdd = Body.InventoryRows - Player.Singleton.Inventory.ItemGrid.GridSizeHeight;
        Player.Singleton.Inventory.ItemGrid.AddRowsToInventory(rowsToAdd);

        Player.Singleton.Inventory.ItemGrid.MaxTotalWeight = Suspension.MaxWeight;
    }
}
