using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CooldownHandlerSaveData
{
    public List<CooldownHandler.ObjectOnCooldown> ObjectsOnCooldown = new();

    public CooldownHandlerSaveData(CooldownHandler cooldownHandler)
    {
        ObjectsOnCooldown.AddRange(cooldownHandler.ObjectsOnCooldown);
    }
}
