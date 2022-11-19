
using UnityEngine;

public static class TradeManager 
{
    private static GameObject _traderPanel;
    public static GameObject TraderPanel => _traderPanel;
    private static Transform _traderPanelContent;
    public static Transform TraderPanelContent => _traderPanelContent;
    private static GoodsPanel _goodsPanel;
    public static GoodsPanel GoodsPanel => _goodsPanel;
    public static PlayersInventory PlayersInventory;
    public static void Init(GameObject traderPanel, Transform content, GoodsPanel goodsPanel, PlayersInventory playersInventory)
    {
        _traderPanel = traderPanel;
        _traderPanelContent = content;
        _goodsPanel = goodsPanel;
        PlayersInventory = playersInventory;
    }
}
