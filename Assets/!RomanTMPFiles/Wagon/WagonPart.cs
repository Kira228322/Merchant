using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonPart : MonoBehaviour
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] private int _level; //для определения прогрессии запчастей
    public Sprite Sprite => _sprite;
    public int Level => _level;

    public virtual void Replace(WagonPart wagonPart)
    {
        _sprite = wagonPart.Sprite;
        _level = wagonPart.Level;
    }
}
