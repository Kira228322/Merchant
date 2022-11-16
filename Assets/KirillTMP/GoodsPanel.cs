using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private Image _icone;
    private Item item;
    public void Init(Item goods, int count)
    {
        item = goods;
        _cost.text = goods.Price.ToString();
        _count.text = count.ToString();
        _icone.sprite = item.Icon;
    }

    private void OnBuyButtonClick()
    {
        // TODO тут, Роман, ты поработаешь в будущем
    }
}
