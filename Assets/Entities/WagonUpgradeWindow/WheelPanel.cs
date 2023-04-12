using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPanel : WagonPartPanel
{
    public override void Init(WagonPart wagonPart)
    {
        Wheel wheel = (Wheel)wagonPart;
        _descriptionText.text = $"Модификатор улучшения дороги: {Math.Round(wheel.QualityModifier - 1, 2) * 100}"; 
        // Я думаю для игра будет довольно не понятно видеть цифру 1.1, что это вообще значит, как 1.1 отличается от 1.15 - не очень ясно
        // Будет лучше, если игрок будет видеть цифру 10 или 15. 
        _image.sprite = wheel.Sprite;
        _partNameText.text = wheel.Name;
        _cost.text = wheel.UpgradePrice.ToString();
    }
}
