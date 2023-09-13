using System;
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

    private NPCWagonUpgrader _wagonUpgrader;
    public void Init(NPCWagonUpgrader wagonUpgrader)
    {
        _wagonUpgrader = wagonUpgrader;
        _body.sprite = Player.Instance.WagonStats.Body.Sprite;
        _suspension.sprite = Player.Instance.WagonStats.Suspension.Sprite;
        _wheel.sprite = Player.Instance.WagonStats.Wheel.Sprite;
        _slotsText.text = $"Количество слотов: {Player.Instance.WagonStats.Body.InventoryRows * 5}";
        _weightText.text = $"Макс. масса груза: {Player.Instance.WagonStats.Suspension.MaxWeight}кг";
        _modifierText.text = $"Модификатор дороги: {Math.Round(Player.Instance.WagonStats.Wheel.QualityModifier - 1, 2) * 100}";
        
        foreach (var upgrades in wagonUpgrader.WagonUpgrades)
        {
            if (upgrades == null) // Если вдруг игрок вкачался на максимум, то в рестоке у wagonUpgradera будут null'ы
                continue;

            if (upgrades is Wheel)
            {
                Instantiate(_wheelPanelPrefab.gameObject, _container).GetComponent<WheelPanel>().Init(upgrades, this);
            }
            else if (upgrades is Suspension)
            {
                Instantiate(_suspensionPanelPrefab.gameObject, _container).GetComponent<SuspensionPanel>().Init(upgrades, this);
            }
            else if (upgrades is Body)
            {
                Instantiate(_bodyPanelPrefab.gameObject, _container).GetComponent<BodyPanel>().Init(upgrades, this);
            } 
        }
    }

    public void OnPlayerBoughtAnything(WagonPart wagonPart)
    {
        _wagonUpgrader.WagonUpgrades.Remove(wagonPart);
        foreach (var part in _wagonUpgrader.WagonUpgrades)
        { // Если игрок покупает деталь на +2 уровня, то деталь на +1 уровень у торговца надо удалить
            if (wagonPart.GetType() == part.GetType())
            {
                if (wagonPart.Level > part.Level)
                    _wagonUpgrader.WagonUpgrades.Remove(part);
                break;
            }
        }
        
        for (int i = 0; i < _container.childCount; i++)
            Destroy(_container.GetChild(i).gameObject);

        _wagonUpgrader.NpcData.CurrentMoney += wagonPart.UpgradePrice;
        Player.Instance.Money -= wagonPart.UpgradePrice;

        Init(_wagonUpgrader);
    }

    public void OnCloseButtonClick()
    {
        TradeManager.Instance.PlayerBlock.alpha = 1;
        _wagonUpgrader.StopInteraction();
        Destroy(gameObject);
    }
}
