using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsSellPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private SlidersController _spoilSlider;
    private int _currentCount;
    private ItemGrid _playerInventoryItemGrid;
    private InventoryItem _item;
    private Trader _trader;

    public void Init(Trader trader, InventoryItem itemToSell, ItemGrid playerInventoryItemGrid)
    {
        _playerInventoryItemGrid = playerInventoryItemGrid;
        _trader = trader;
        _item = itemToSell;
        _currentCount = itemToSell.CurrentItemsInAStack;
        _cost.text = _item.ItemData.Price.ToString(); //��� �� ����������� ���� ��� ����� ����������� ��������
        _countText.text = _currentCount.ToString();
        _icon.sprite = _item.ItemData.Icon;
        _itemName.text = _item.ItemData.Name;

        if (itemToSell.ItemData.IsPerishable)
        {
            _spoilSlider.gameObject.SetActive(true);
            _spoilSlider.SetValue(_item.ItemData.DaysToSpoil - _item.BoughtDaysAgo, _item.ItemData.DaysToSpoil);
            if (_item.BoughtDaysAgo > _item.ItemData.DaysToHalfSpoil)
            {
                Color yellow = new(178f / 255, 179f / 255, 73f / 255);
                _spoilSlider.SetColour(yellow);
            }
        }
    }

    public void OnSellButtonClick()
    {
        //������� ����� �� �����, ��������� ������� �� �����, ��������� ������ � ��������, ��������� CountToBuy � ��������

        //��� ����� ��������, ��� ������� "�����" ���� ���� ��� �������� ������. 
        _item.CurrentItemsInAStack--;
        if (_item.CurrentItemsInAStack <= 0)
        {
            InventoryController.Instance.DestroyItem(_playerInventoryItemGrid, _item);
        }

        /* ���� ��� ������ ���� �� ����� ������� �������� ����� �� ������, ��:
         * 1) ����� ���-�� ���������� � ������������, ����� ��� ���� - ������ ������, ������� ������
         * 2) ������� �������� ��������� � InventoryItem, ������� � �������� � Item 
        GameObject tradersGoods = Instantiate(TradeManager.Singleton.GoodsPanelPrefub.gameObject, TradeManager.Singleton.BuyPanelContent);
        tradersGoods.GetComponent<GoodsBuyPanel>().Init(_trader, _item.ItemData, 1);
        */

    }
}
