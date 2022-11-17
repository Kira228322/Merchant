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
        TradeManager.TraderPanel.SetActive(true);
        for (int i =  TradeManager.TraderPanelContent.childCount -1; i >= 0; i--)
            Destroy(TradeManager.TraderPanelContent.GetChild(i).gameObject);
        
        for (int i = 0; i < _trader.Goods.Count; i++)
        {
            GameObject tradersGoods = Instantiate(TradeManager.GoodsPanel.gameObject, TradeManager.TraderPanelContent);
            tradersGoods.GetComponent<GoodsPanel>().Init(_trader, _trader.Goods[i], _trader.CountOfGood[i]);
        }
        Destroy(gameObject);
    }

    public void OnSellButtonClick()
    {
        
    }
}
