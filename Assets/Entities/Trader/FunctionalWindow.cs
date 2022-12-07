using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FunctionalWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _merchantName;

    private Trader _trader;

    
    
    public void Init(Trader trader)
    {
        _trader = trader;
        
        _merchantName.text = _trader.Name; 
    }
    
    public void OnTalkButtonClick()
    {
        
    }

    public void OnBuyButtonClick()
    {
        Player.Singleton.Inventory.InventoryPanel.SetActive(true);
        TradeManager.Singleton.BuyPanel.SetActive(true);
        for (int i =  TradeManager.Singleton.BuyPanelContent.childCount -1; i >= 0; i--)
            Destroy(TradeManager.Singleton.BuyPanelContent.GetChild(i).gameObject);
        
        for (int i = 0; i < _trader.Goods.Count; i++)
        {
            GameObject tradersGoods = Instantiate(TradeManager.Singleton.GoodsPanelPrefub.gameObject, TradeManager.Singleton.BuyPanelContent);
            tradersGoods.GetComponent<GoodsPanel>().Init(_trader, _trader.Goods[i], _trader.CountOfGood[i]);
        }
        Destroy(gameObject);
    }

    public void OnSellButtonClick()
    {
        Player.Singleton.Inventory.InventoryPanel.SetActive(true);
        Player.Singleton.Inventory.ShowItemsSellValue(true);
        TradeManager.Singleton.SellPanel.SetActive(true);
        Destroy(gameObject);
    }
}
