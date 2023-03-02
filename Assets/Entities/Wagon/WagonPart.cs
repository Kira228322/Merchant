using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonPart : ScriptableObject
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] private int _level; //для определения прогрессии запчастей
    public Sprite Sprite => _sprite;
    public int Level => _level;


}
