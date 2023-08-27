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
    


    private void Start()
    {
        _currentTotalWeight = _playersInventory.CurrentTotalWeight;
        _maxTotalWeight = _playersInventory.MaxTotalWeight;
        
        
    }
    
    private void OnEnable()
    {
        _playersInventory.WeightChanged += OnWeightChanged;
        Refresh();
        _goldText.text = Player.Instance.Money.ToString();

    }
    private void OnDisable()
    {
        _playersInventory.WeightChanged -= OnWeightChanged;
    }
    private void Refresh()
    {
        _weightText.text = _currentTotalWeight.ToString("F1") + " / " + _maxTotalWeight.ToString("F1"); //.ToString("F1") округляет до 1 знаков после запятой
        if (_playersInventory.IsOverencumbered)
        {
            _weightText.color = Color.red;
        }
        else
        {
            Color brown = new(125f / 255, 97f / 255, 65f / 255);
            _weightText.color = brown;
        }
    }
    private void OnWeightChanged(float currentWeight, float maxWeight)
    {
        _currentTotalWeight = currentWeight;
        _maxTotalWeight = maxWeight;
        Refresh();
    }
}
