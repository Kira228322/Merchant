using System;

public class WheelPanel : WagonPartPanel
{
    public override void Init(WagonPart wagonPart, WagonUpgradeWindow window)
    {
        _window = window;
        _wagonPart = wagonPart;
        Wheel wheel = (Wheel)wagonPart;
        _descriptionText.text = $"����������� ��������� ������: {Math.Round(wheel.QualityModifier - 1, 2) * 100}";
        // � ����� ��� ���� ����� �������� �� ������� ������ ����� 1.1, ��� ��� ������ ������, ��� 1.1 ���������� �� 1.15 - �� ����� ����
        // ����� �����, ���� ����� ����� ������ ����� 10 ��� 15. 
        _image.sprite = wheel.Sprite;
        _partNameText.text = wheel.Name;
        _cost.text = wheel.UpgradePrice.ToString();

        if (Player.Instance.WagonStats.Wheel.Level >= _wagonPart.Level)
            _installButton.interactable = false;
    }


}
