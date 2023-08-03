using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCWagonUpgrader : Npc
{
    public List<WagonPart> WagonUpgrades = new List<WagonPart>();

    protected void Start()
    {
        WagonUpgraderRestock(); // TODO это ради теста
    }
    

    private void WagonUpgraderRestock()
    {
        WagonUpgrades.Clear();
        
        if (TravelEventHandler.EventFire(75, true, Player.Instance.Statistics.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Body>(Player.Instance.WagonStats.Body.Level + 1));
        
        if (TravelEventHandler.EventFire(75, true, Player.Instance.Statistics.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Suspension>(Player.Instance.WagonStats.Suspension.Level + 1));
        
        if (TravelEventHandler.EventFire(75, true, Player.Instance.Statistics.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Wheel>(Player.Instance.WagonStats.Wheel.Level + 1));
        
        if (TravelEventHandler.EventFire(15, true, Player.Instance.Statistics.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Body>(Player.Instance.WagonStats.Body.Level + 2));
        
        if (TravelEventHandler.EventFire(15, true, Player.Instance.Statistics.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Suspension>(Player.Instance.WagonStats.Suspension.Level + 2));
        
        if (TravelEventHandler.EventFire(15, true, Player.Instance.Statistics.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Wheel>(Player.Instance.WagonStats.Wheel.Level + 2));
        
    }
    
    
}
