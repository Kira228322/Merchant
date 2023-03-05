using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWagonStatsSaveData
{
    public string WheelName;
    public string BodyName;
    public string SuspensionName;

    public PlayerWagonStatsSaveData(PlayerWagonStats wagonStats)
    {
        WheelName = wagonStats.Wheel.Name;
        BodyName = wagonStats.Body.Name;
        SuspensionName = wagonStats.Suspension.Name;
    }
}
