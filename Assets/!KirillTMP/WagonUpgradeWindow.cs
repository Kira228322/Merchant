using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WagonUpgradeWindow : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private BodyPanel _bodyPanelPrefub;
    [SerializeField] private WheelPanel _wheelPanelPrefub;
    [SerializeField] private SuspensionPanel _suspensionPanelPrefub;
    [SerializeField] private Image _body;
    [SerializeField] private Image _suspension;
    [SerializeField] private Image _wheel;
    [SerializeField] private TMP_Text _slotsText;
    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private TMP_Text _modifierText;
    public void Init(NPCWagonUpgrader wagonUpgrader)
    {
        _body.sprite = Player.Instance.WagonStats.Body.Sprite;
        _suspension.sprite = Player.Instance.WagonStats.Suspension.Sprite;
        _wheel.sprite = Player.Instance.WagonStats.Wheel.Sprite;
        _slotsText.text = $"Колличество слотов: {Player.Instance.WagonStats.Body.InventoryRows*5}";
        _weightText.text = $"Макс. масса груза: {Player.Instance.WagonStats.Suspension.MaxWeight}кг";
        _modifierText.text = $"Модефикатор дороги: {Math.Round(Player.Instance.WagonStats.Wheel.QualityModifier - 1, 2) * 100}";
        
        foreach (var upgrades in wagonUpgrader.WagonUpgrades)
        {
            if (upgrades == null) // Если вдруг игрок вкачался на максимум, то в рестоке у wagonUpgradera будут null'ы
                continue;

            if (upgrades is Wheel)
            {
                Instantiate(_wheelPanelPrefub.gameObject, _container).GetComponent<WheelPanel>().Init(upgrades);
            }
            else if (upgrades is Suspension)
            {
                Instantiate(_suspensionPanelPrefub.gameObject, _container).GetComponent<SuspensionPanel>().Init(upgrades);
            }
            else if (upgrades is Body)
            {
                Instantiate(_bodyPanelPrefub.gameObject, _container).GetComponent<BodyPanel>().Init(upgrades);
            } 
        }
    }

    public void OnCloseButtonClick()
    {
        TradeManager.Instance.PlayerBlock.alpha = 1;
        Destroy(gameObject);
    }
}
