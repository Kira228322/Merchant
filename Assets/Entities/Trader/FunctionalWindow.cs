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
        Destroy(gameObject);
    }

    public void OnTradeButtonClick()
    {
        OpenBuyPanel();
        OpenSellPanel();
        OpenInventory();
        Destroy(gameObject);
    }
    private void OpenBuyPanel()
    {

        TradeManager.Singleton.BuyPanel.SetActive(true);
        for (int i = TradeManager.Singleton.BuyPanelContent.childCount - 1; i >= 0; i--)
            Destroy(TradeManager.Singleton.BuyPanelContent.GetChild(i).gameObject);

        for (int i = 0; i < _trader.Goods.Count; i++)
        {
            GameObject tradersGoods = Instantiate(TradeManager.Singleton.GoodsBuyPanelPrefab.gameObject, TradeManager.Singleton.BuyPanelContent);
            tradersGoods.GetComponent<GoodsBuyPanel>().Init(_trader, _trader.Goods[i], 0f, true, _trader.CountOfGood[i]);
        }

        //InventoryController.Instance.enabled = false;

    }
    private void OpenSellPanel()
    {
        
        TradeManager.Singleton.SellPanel.SetActive(true);

        for (int i = TradeManager.Singleton.SellPanelContent.childCount - 1; i >= 0; i--)
            Destroy(TradeManager.Singleton.SellPanelContent.GetChild(i).gameObject);

        for (int i = 0; i < Player.Singleton.Inventory.ItemList.Count; i++)
        {
            GameObject tradersGoods = Instantiate(TradeManager.Singleton.GoodsSellPanelPrefab.gameObject, TradeManager.Singleton.SellPanelContent);
            tradersGoods.GetComponent<GoodsSellPanel>().Init(_trader,Player.Singleton.Inventory.ItemList[i], Player.Singleton.Inventory.GetComponent<ItemGrid>());
        }
    }
    private void OpenInventory()
    {
        Player.Singleton.Inventory.InventoryPanel.SetActive(true);
    }
}
