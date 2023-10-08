using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionPanel : WagonPartPanel
{
    public override void Init(WagonPart wagonPart, WagonUpgradeWindow window)
    {
        _window = window;
        _wagonPart = wagonPart;
        Suspension suspension = (Suspension)wagonPart;
        _descriptionText.text = $"Предельно допустимая масса: {suspension.MaxWeight}кг";
        _image.sprite = suspension.Sprite;
        _partNameText.text = suspension.Name;
        _cost.text = suspension.UpgradePrice.ToString();

        if (Player.Instance.WagonStats.Suspension.Level >= _wagonPart.Level)
            _installButton.interactable = false;
    }

}
