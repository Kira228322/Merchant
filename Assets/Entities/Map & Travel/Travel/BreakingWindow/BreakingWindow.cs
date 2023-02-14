using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakingWindow : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private TMP_Text _totalPriceText;
    [SerializeField] private ItemInBreakingWindow _itemPrefub;
    public void Init(List<InventoryItem> items)
    {
        int totalPrice = 0;
        int count = 1;
        
        for (int i = 0; i != items.Count;)
        {
            for (int j = 1; j < items.Count; j++)
            {
                if (items[0].ItemData.Name == items[j].ItemData.Name)
                {
                    count++;
                    items.RemoveAt(j);
                }
            }
            Instantiate(_itemPrefub.gameObject, _container).GetComponent<ItemInBreakingWindow>().
                Init(items[i].ItemData.Icon, count, items[i].ItemData.Price * count);
            totalPrice += items[i].ItemData.Price * count;
            count = 1;
            items.RemoveAt(i);
        }

        _totalPriceText.text += totalPrice;
    }
}
