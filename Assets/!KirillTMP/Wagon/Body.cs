using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    private int _inventoryRows;
    public Sprite Sprite => _sprite;
    public int InventoryRows => _inventoryRows;
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
