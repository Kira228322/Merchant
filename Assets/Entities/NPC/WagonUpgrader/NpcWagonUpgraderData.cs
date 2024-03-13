using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "newWagonUpgraderData", menuName = "NPCs/WagonUpgraderData")]
public class NpcWagonUpgraderData : NpcData, IResetOnExitPlaymode, ISaveable<NpcWagonUpgraderSaveData>
{

    public List<WagonPart> CurrentWagonUpgrades = new();

    public void RestockParts()
    {
        CurrentWagonUpgrades.Clear();

        if (TravelEventHandler.EventFire(75, true, Player.Instance.Statistics.Diplomacy))
            CurrentWagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Body>(Player.Instance.WagonStats.Body.Level + 1));

        if (TravelEventHandler.EventFire(75, true, Player.Instance.Statistics.Diplomacy))
            CurrentWagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Suspension>(Player.Instance.WagonStats.Suspension.Level + 1));

        if (TravelEventHandler.EventFire(75, true, Player.Instance.Statistics.Diplomacy))
            CurrentWagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Wheel>(Player.Instance.WagonStats.Wheel.Level + 1));

        if (TravelEventHandler.EventFire(15, true, Player.Instance.Statistics.Diplomacy))
            CurrentWagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Body>(Player.Instance.WagonStats.Body.Level + 2));

        if (TravelEventHandler.EventFire(15, true, Player.Instance.Statistics.Diplomacy))
            CurrentWagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Suspension>(Player.Instance.WagonStats.Suspension.Level + 2));

        if (TravelEventHandler.EventFire(15, true, Player.Instance.Statistics.Diplomacy))
            CurrentWagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Wheel>(Player.Instance.WagonStats.Wheel.Level + 2));

    }
    void IResetOnExitPlaymode.ResetOnExitPlaymode()
    {
        ResetOnExitPlaymode();
        CurrentWagonUpgrades.Clear();
    }
    void ISaveable<NpcWagonUpgraderSaveData>.LoadData(NpcWagonUpgraderSaveData data)
    {
        LoadData(data);
        CurrentWagonUpgrades = data.CurrentUpgrades.Select(upgrade => WagonPartDatabase.GetWagonPart(upgrade)).ToList();
    }

    NpcWagonUpgraderSaveData ISaveable<NpcWagonUpgraderSaveData>.SaveData()
    {
        NpcWagonUpgraderSaveData saveData = new(ID, CurrentMoney, CurrentWagonUpgrades);
        return saveData;
    }
}
