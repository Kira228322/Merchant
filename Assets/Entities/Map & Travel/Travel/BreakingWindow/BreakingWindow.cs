using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakingWindow : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private TMP_Text _totalPriceText;
    [SerializeField] private ItemInBreakingWindow _itemPrefab;
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
                    j--;
                }
            }
            Instantiate(_itemPrefab.gameObject, _container).GetComponent<ItemInBreakingWindow>().
                Init(items[i].ItemData.Icon, count, items[i].ItemData.Price);
            totalPrice += items[i].ItemData.Price * count;
            count = 1;
            items.RemoveAt(i);
        }

        _totalPriceText.text = $"Общая стоимость потерянных товаров: {totalPrice}";
    }

    public void OnButtonClick()
    {
        GameTime.SetTimeScale(1);
        MapManager.TransitionToVillageScene();
        FindObjectOfType<TravelTimeCounter>().gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
