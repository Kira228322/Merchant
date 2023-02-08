using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body", menuName = "WagonParts/Body")]
public class Body : WagonPart
{
    [SerializeField] [Header("Сколько ВСЕГО должно быть рядов в инвентаре")] private int _inventoryRows;
    public int InventoryRows => _inventoryRows;
}
