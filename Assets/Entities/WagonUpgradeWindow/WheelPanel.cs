using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPanel : WagonPartPanel
{
    public override void Init(WagonPart wagonPart)
    {
        Wheel wheel = (Wheel)wagonPart;
        _descriptionText.text = $"����������� ��������� ������: {Math.Round(wheel.QualityModifier - 1, 2) * 100}"; 
        // � ����� ��� ���� ����� �������� �� ������� ������ ����� 1.1, ��� ��� ������ ������, ��� 1.1 ���������� �� 1.15 - �� ����� ����
        // ����� �����, ���� ����� ����� ������ ����� 10 ��� 15. 
        _image.sprite = wheel.Sprite;
        _partNameText.text = wheel.Name;
        _cost.text = wheel.UpgradePrice.ToString();
    }
}
