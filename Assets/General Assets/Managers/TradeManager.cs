
using System;
using UnityEngine;

public  class TradeManager : MonoBehaviour
{
    public static TradeManager Singleton;
    [SerializeField] private  GameObject _BuyPanel;
    public  GameObject BuyPanel => _BuyPanel;
    [SerializeField] private  GameObject _sellPanel;
    public  GameObject SellPanel => _sellPanel;
    [SerializeField] private  Transform _BuyPanelContent;
    public  Transform BuyPanelContent => _BuyPanelContent;
    [SerializeField] private  GoodsPanel _goodsPanelPrefub;
    public  GoodsPanel GoodsPanelPrefub => _goodsPanelPrefub;

    private void Start()
    {
        Singleton = this;
    }
}
