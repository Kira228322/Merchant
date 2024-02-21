using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CooldownHandlerSaveData
{
    public List<CooldownHandler.ObjectOnCooldown> ObjectsOnCooldown;

    public CooldownHandlerSaveData(CooldownHandler cooldownHandler)
    {
        ObjectsOnCooldown = new(cooldownHandler.ObjectsOnCooldown);
    }
}
