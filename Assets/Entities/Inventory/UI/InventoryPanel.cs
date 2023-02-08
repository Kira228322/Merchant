using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    //Отображение Money и Weight в окне инвентаря

    [SerializeField] private PlayersInventory _playersInventory;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _weightText;

    private float _currentTotalWeight;
    private float _maxTotalWeight;
    private int _money;


    private void Start()
    {
        _currentTotalWeight = _playersInventory.CurrentTotalWeight;
        _maxTotalWeight = _playersInventory.MaxTotalWeight;
        _money = Player.Singleton.Money;
        Refresh();
    }

    private void OnPlayerSingletonChanged()
    {
        // https://forum.unity.com/threads/do-i-need-to-unsubscribe-if-an-object-containing-event-handler-is-destroyed.1062824/
        // Пост №4. Сказано, что если обладатель ивента уничтожается, то нет нужды отписываться. Надеюсь, правда. Таким образом,
        // просто переподписываюсь на тот же ивент.

        Player.Singleton.MoneyChanged += OnMoneyChanged;
    }

    private void OnEnable()
    {
        Player.PlayerSingletonChanged += OnPlayerSingletonChanged;
        Player.Singleton.MoneyChanged += OnMoneyChanged;
        _playersInventory.WeightChanged += OnWeightChanged;
        
    }
    private void OnDisable()
    {
        Player.PlayerSingletonChanged -= OnPlayerSingletonChanged;
        Player.Singleton.MoneyChanged -= OnMoneyChanged;
        _playersInventory.WeightChanged -= OnWeightChanged;
    }
    private void Refresh()
    {
        _goldText.text = _money.ToString();
        _weightText.text = _currentTotalWeight.ToString("F1") + " / " + _maxTotalWeight.ToString("F1"); //.ToString("F1") округляет до 1 знаков после запятой
        if (_playersInventory.IsOverencumbered)
        {
            _weightText.color = Color.red;
        }
        else
        {
            Color brown = new Color(125f, 97f, 65f);
            _weightText.color = brown;
            //Не работает, почему? Становится просто белым
        }
    }
    private void OnMoneyChanged(int money)
    {
        _money = money;
        Refresh();
    }
    private void OnWeightChanged(float currentWeight, float maxWeight)
    {
        _currentTotalWeight = currentWeight;
        _maxTotalWeight = maxWeight;
        Refresh();
    }
}
