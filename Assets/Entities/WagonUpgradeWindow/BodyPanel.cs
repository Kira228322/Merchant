using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPanel : WagonPartPanel
{
    public override void Init(WagonPart wagonPart)
    {
        Body body = (Body)wagonPart;
        _descriptionText.text = $"Количество доступных слотов: {body.InventoryRows*5}";
        _image.sprite = body.Sprite;
        _partNameText.text = body.Name;
        _cost.text = body.UpgradePrice.ToString();
    }
}
