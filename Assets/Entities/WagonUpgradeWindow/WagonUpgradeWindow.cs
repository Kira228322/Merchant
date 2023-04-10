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
    [SerializeField] private BodyPanel _bodyPanelPrefab;
    [SerializeField] private WheelPanel _wheelPanelPrefab;
    [SerializeField] private SuspensionPanel _suspensionPanelPrefab;
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
        _slotsText.text = $"���������� ������: {Player.Instance.WagonStats.Body.InventoryRows*5}";
        _weightText.text = $"����. ����� �����: {Player.Instance.WagonStats.Suspension.MaxWeight}��";
        _modifierText.text = $"����������� ������: {Math.Round(Player.Instance.WagonStats.Wheel.QualityModifier - 1, 2) * 100}";
        
        foreach (var upgrades in wagonUpgrader.WagonUpgrades)
        {
            if (upgrades == null) // ���� ����� ����� �������� �� ��������, �� � ������� � wagonUpgradera ����� null'�
                continue;

            if (upgrades is Wheel)
            {
                Instantiate(_wheelPanelPrefab.gameObject, _container).GetComponent<WheelPanel>().Init(upgrades);
            }
            else if (upgrades is Suspension)
            {
                Instantiate(_suspensionPanelPrefab.gameObject, _container).GetComponent<SuspensionPanel>().Init(upgrades);
            }
            else if (upgrades is Body)
            {
                Instantiate(_bodyPanelPrefab.gameObject, _container).GetComponent<BodyPanel>().Init(upgrades);
            } 
        }
    }

    public void OnCloseButtonClick()
    {
        TradeManager.Instance.PlayerBlock.alpha = 1;
        Destroy(gameObject);
    }
}
