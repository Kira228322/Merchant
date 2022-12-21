
using System;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Singleton;
    [SerializeField] private CanvasGroup _playerBlock;
    [SerializeField] private GameObject _closeTradeButton;
    [SerializeField] private GameObject _BuyPanel;
    public GameObject BuyPanel => _BuyPanel;
    [SerializeField] private GameObject _sellPanel;
    public GameObject SellPanel => _sellPanel;
    [SerializeField] private Transform _buyPanelContent;
    public Transform BuyPanelContent => _buyPanelContent;
    [SerializeField] private GoodsBuyPanel _goodsBuyPanelPrefab;
    public GoodsBuyPanel GoodsBuyPanelPrefab => _goodsBuyPanelPrefab;
    [SerializeField] private GoodsSellPanel _goodsSellPanelPrefab;
    public GoodsSellPanel GoodsSellPanelPrefab => _goodsSellPanelPrefab;
    [SerializeField] private Transform _sellPanelContent;
    public Transform SellPanelContent => _sellPanelContent;

    private void Start()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
        }
    }
    public void OpenTradeWindow(Trader trader)
    {
        InventoryController.Instance.enabled = false;
        _playerBlock.alpha = 0;
        _closeTradeButton.SetActive(true);
        OpenBuyPanel(trader);
        OpenSellPanel(trader);
        OpenInventory();
    }
    public void CloseTradeWindow()
    {
        InventoryController.Instance.enabled = true;
        _playerBlock.alpha = 1;
        _closeTradeButton.SetActive(false);
        Singleton.BuyPanel.SetActive(false);
        Singleton.SellPanel.SetActive(false);
        Player.Singleton.Inventory.InventoryPanel.SetActive(false);
    }
    private void OpenBuyPanel(Trader trader)
    {

        Singleton.BuyPanel.SetActive(true);
        for (int i = Singleton.BuyPanelContent.childCount - 1; i >= 0; i--)
            Destroy(Singleton.BuyPanelContent.GetChild(i).gameObject);

        for (int i = 0; i < trader.Goods.Count; i++)
        {
            GameObject tradersGoods = Instantiate(GoodsBuyPanelPrefab.gameObject, BuyPanelContent);
            tradersGoods.GetComponent<GoodsBuyPanel>().Init(trader, trader.Goods[i], 0f, true, trader.CountOfGood[i]);
        }

    }
    private void OpenSellPanel(Trader trader)
    {

        Singleton.SellPanel.SetActive(true);

        for (int i = Singleton.SellPanelContent.childCount - 1; i >= 0; i--)
            Destroy(Singleton.SellPanelContent.GetChild(i).gameObject);

        for (int i = 0; i < Player.Singleton.Inventory.ItemList.Count; i++)
        {
            GameObject tradersGoods = Instantiate(GoodsSellPanelPrefab.gameObject, SellPanelContent);
            tradersGoods.GetComponent<GoodsSellPanel>().Init(trader, Player.Singleton.Inventory.ItemList[i], Player.Singleton.Inventory.GetComponent<ItemGrid>());
        }
    }
    private void OpenInventory()
    {
        Player.Singleton.Inventory.InventoryPanel.SetActive(true);
    }
}
