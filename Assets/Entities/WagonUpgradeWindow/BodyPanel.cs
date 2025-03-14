public class BodyPanel : WagonPartPanel
{
    public override void Init(WagonPart wagonPart, WagonUpgradeWindow window)
    {
        _window = window;
        _wagonPart = wagonPart;
        Body body = (Body)wagonPart;
        _descriptionText.text = $"Количество доступных слотов: {body.InventoryRows * 5}";
        _image.sprite = body.Sprite;
        _partNameText.text = body.Name;
        _cost.text = body.UpgradePrice.ToString();

        if (Player.Instance.WagonStats.Body.Level >= _wagonPart.Level)
            _installButton.interactable = false;
    }

}
