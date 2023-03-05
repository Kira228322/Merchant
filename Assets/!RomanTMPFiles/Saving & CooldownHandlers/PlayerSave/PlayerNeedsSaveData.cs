using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerNeedsSaveData
{
    public int currentHunger;
    public int currentSleep;

    public int maxHunger;
    public int maxSleep;

    public PlayerNeedsSaveData(PlayerNeeds needs)
    {
        currentHunger = needs.CurrentHunger;
        currentSleep = needs.CurrentSleep;
        maxHunger = needs.MaxHunger;
        maxSleep = needs.MaxSleep;
    }
}
