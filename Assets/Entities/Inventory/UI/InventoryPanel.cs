using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    //����������� Money � Weight � ���� ���������

    [SerializeField] private ItemGrid _inventoryItemGrid;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _weightText;

    private float _currentTotalWeight;
    private float _maxTotalWeight;
    private int _money;


    private void Start()
    {
        _currentTotalWeight = _inventoryItemGrid.CurrentTotalWeight;
        _maxTotalWeight = _inventoryItemGrid.MaxTotalWeight;
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
        
    }
    private void OnDisable()
    {
        Player.PlayerSingletonChanged -= OnPlayerSingletonChanged;
        Player.Singleton.MoneyChanged -= OnMoneyChanged;
        
    }
    private void Refresh()
    {
        _goldText.text = _money.ToString();
        _weightText.text = _currentTotalWeight.ToString("F1") + " / " + _maxTotalWeight.ToString("F1"); //.ToString("F1") ��������� �� 1 ������ ����� �������
    }
    private void OnMoneyChanged(int money)
    {
        _money = money;
        Refresh();
    }
}
