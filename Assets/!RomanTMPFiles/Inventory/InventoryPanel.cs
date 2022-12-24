using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    //����������� Money � TotalWeight � ���� ���������

    [SerializeField] private ItemGrid _inventoryItemGrid;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _totalWeightText;

    public float TotalWeight;
    private int _money;


    private void Start()
    {
        TotalWeight = CalculateWeight();
        _money = Player.Singleton.Money;
        Refresh();
    }

    private void OnPlayerSingletonChanged()
    {
        // https://forum.unity.com/threads/do-i-need-to-unsubscribe-if-an-object-containing-event-handler-is-destroyed.1062824/
        // ���� �4. �������, ��� ���� ���������� ������ ������������, �� ��� ����� ������������. �������, ������. ����� �������,
        // ������ ���������������� �� ��� �� �����.

        Player.Singleton.MoneyChanged += OnMoneyChanged;
    }

    private void OnEnable()
    {
        Player.PlayerSingletonChanged += OnPlayerSingletonChanged;
        Player.Singleton.MoneyChanged += OnMoneyChanged;
        _inventoryItemGrid.ItemPlacedInTheGrid += AddWeight;
        _inventoryItemGrid.ItemPlacedInTheStack += AddWeight;
        _inventoryItemGrid.ItemRemovedFromTheGrid += SubstractWeight;
        _inventoryItemGrid.ItemsRemovedFromTheStack += SubstractWeight;
        
    }
    private void OnDisable()
    {
        Player.PlayerSingletonChanged -= OnPlayerSingletonChanged;
        Player.Singleton.MoneyChanged -= OnMoneyChanged;
        _inventoryItemGrid.ItemPlacedInTheGrid -= AddWeight;
        _inventoryItemGrid.ItemPlacedInTheStack -= AddWeight;
        _inventoryItemGrid.ItemRemovedFromTheGrid -= SubstractWeight;
        _inventoryItemGrid.ItemsRemovedFromTheStack -= SubstractWeight;
        
    }
    private float CalculateWeight()
    {
        float totalWeight = 0;
        foreach (var item in Player.Singleton.Inventory.ItemList)
        {
            totalWeight += item.ItemData.Weight * item.CurrentItemsInAStack;
        }
        return totalWeight;
    }
    private void Refresh()
    {
        _goldText.text = _money.ToString();
        _totalWeightText.text = TotalWeight.ToString("F1"); //.ToString("F1") ��������� �� 1 ������ ����� �������
    }
    private void AddWeight(InventoryItem item)
    {
        TotalWeight += item.ItemData.Weight * item.CurrentItemsInAStack;
        Refresh();
    }
    private void AddWeight(InventoryItem item, int howManyWereInserted)
    {
        TotalWeight += item.ItemData.Weight * howManyWereInserted;
        Refresh();
    }
    private void SubstractWeight(InventoryItem item)
    {
        TotalWeight -= item.ItemData.Weight * item.CurrentItemsInAStack;
        Refresh();
    }
    private void SubstractWeight(InventoryItem item, int amountRemoved)
    {
        TotalWeight -= item.ItemData.Weight * amountRemoved;
        Refresh();
    }
    private void OnMoneyChanged(int money)
    {
        _money = money;
        Refresh();
    }
}
