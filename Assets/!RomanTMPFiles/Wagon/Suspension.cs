using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    private int _weight;
    public Sprite Sprite => _sprite;
    public int Weight => _weight;
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
