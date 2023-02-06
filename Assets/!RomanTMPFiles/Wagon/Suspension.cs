using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension: WagonPart
{
    [SerializeField] private int _weight;
    public int MaxWeight => _weight;
    public override void Replace(WagonPart wagonPart)
    {
        base.Replace(wagonPart);
        Suspension suspension = wagonPart as Suspension;
        _weight = suspension.MaxWeight;
    }
}
