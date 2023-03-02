using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Suspension", menuName = "WagonParts/Suspension")]
public class Suspension: WagonPart
{
    [SerializeField] private int _weight;
    public int MaxWeight => _weight;
}
