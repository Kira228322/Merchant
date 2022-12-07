using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _itemName;
    private int _currentCount;
    private Item _item;
    private InventoryController _inventoryController;
    private Trader _trader;

    private void Start()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
    }
    public void Init(Trader trader,Item goods, int count)
    {
        _trader = trader;
        _item = goods;
        _currentCount = count;
        _cost.text = goods.Price.ToString();
        _countText.text = _currentCount.ToString();
        _icon.sprite = _item.Icon;
        _itemName.text = goods.Name;
    }

    public void OnBuyButtonClick()
    {
        if (_currentCount > 0)
        {
            if (!_inventoryController.TryCreateAndInsertItem(Player.Singleton.Inventory.GetComponent<ItemGrid>(), _item, 1, 0f, false, true))
            {
                if (!_inventoryController.TryCreateAndInsertItemRotated(Player.Singleton.Inventory.GetComponent<ItemGrid>(), _item, 1, 0f, true))
                {
                    return;
                }
            }
            _currentCount--;
            _countText.text = _currentCount.ToString();
            _trader.SellItem(_item);
        }
    }
}
