using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonPart : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] protected Sprite _sprite;
    [SerializeField] private int _level; //для определения прогрессии запчастей
    [SerializeField] protected int _upgradePrice;

    public string Name => _name;
    public Sprite Sprite => _sprite;
    public int Level => _level;
    public int UpgradePrice => _upgradePrice;

}
