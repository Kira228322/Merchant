
using System;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Instance;
    [SerializeField] private CanvasGroup _playerBlock;
    [SerializeField] private GameObject _closeTradeButton;
    [SerializeField] private GameObject _BuyPanel;
    public CanvasGroup PlayerBlock => _playerBlock;
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
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void OpenTradeWindow(NPCTrader trader)
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
        Instance.BuyPanel.SetActive(false);
        Instance.SellPanel.SetActive(false);
        Player.Instance.Inventory.InventoryPanel.SetActive(false);
    }
    private void OpenBuyPanel(NPCTrader trader)
    {
        Instance.BuyPanel.SetActive(true);
        for (int i = Instance.BuyPanelContent.childCount - 1; i >= 0; i--)
            Destroy(Instance.BuyPanelContent.GetChild(i).gameObject);

        for (int i = 0; i < trader.Goods.Count; i++)
        {
            if (trader.Goods[i].CurrentCount <= 0) continue;
            GameObject tradersGoods = Instantiate(GoodsBuyPanelPrefab.gameObject, BuyPanelContent);
            tradersGoods.GetComponent<GoodsBuyPanel>().Init(trader, trader.Goods[i], 0f, true, trader.Goods[i].CurrentCount);
        }

        for (int i = 0; i < trader.AdditiveGoods.Count; i++)
        {
            if (trader.AdditiveGoods[i].CurrentCount <= 0) continue;
            GameObject tradersGoods = Instantiate(GoodsBuyPanelPrefab.gameObject, BuyPanelContent);
            tradersGoods.GetComponent<GoodsBuyPanel>().Init(trader, trader.AdditiveGoods[i], 0f, true, trader.AdditiveGoods[i].CurrentCount);
        }

    }
    private void OpenSellPanel(NPCTrader trader)
    {
        Instance.SellPanel.SetActive(true);

        for (int i = Instance.SellPanelContent.childCount - 1; i >= 0; i--)
            Destroy(Instance.SellPanelContent.GetChild(i).gameObject);

        for (int i = 0; i < Player.Instance.Inventory.ItemList.Count; i++)
        {
            GameObject tradersGoods = Instantiate(GoodsSellPanelPrefab.gameObject, SellPanelContent);
            tradersGoods.GetComponent<GoodsSellPanel>().Init(trader, Player.Instance.Inventory.ItemList[i], Player.Instance.Inventory.ItemGrid);
        }
    }
    private void OpenInventory()
    {
        Player.Instance.Inventory.InventoryPanel.SetActive(true);
    }
}
