
using UnityEngine;

public static class TradeManager 
{
    private static GameObject _traderPanel;
    public static GameObject TraderPanel => _traderPanel;
    private static GameObject _sellPanel;
    public static GameObject SellPanel => _sellPanel;
    private static Transform _traderPanelContent;
    public static Transform TraderPanelContent => _traderPanelContent;
    private static GoodsPanel _goodsPanel;
    public static GoodsPanel GoodsPanel => _goodsPanel;
    public static PlayersInventory PlayersInventory;
    public static void Init(GameObject traderPanel, GameObject sellPanel, Transform content, GoodsPanel goodsPanel, PlayersInventory playersInventory)
    {
        _traderPanel = traderPanel;
        _sellPanel = sellPanel;
        _traderPanelContent = content;
        _goodsPanel = goodsPanel;
        PlayersInventory = playersInventory;
    }
}
