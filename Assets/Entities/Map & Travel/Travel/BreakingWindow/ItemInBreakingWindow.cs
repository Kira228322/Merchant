using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInBreakingWindow : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private TMP_Text _totalPrice;

    public void Init(Sprite sprite, int count, int priceOfOneGoods)
    {
        _icon.sprite = sprite;
        _count.text += count;
        _totalPrice.text += count * priceOfOneGoods;
    }
}
