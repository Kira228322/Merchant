
using System;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Singleton;
    [SerializeField] private GameObject _BuyPanel;
    public GameObject BuyPanel => _BuyPanel;
    [SerializeField] private GameObject _sellPanel;
    public GameObject SellPanel => _sellPanel;
    [SerializeField] private Transform _BuyPanelContent;
    public Transform BuyPanelContent => _BuyPanelContent;
    [SerializeField] private GoodsBuyPanel _goodsBuyPanelPrefab;
    public GoodsBuyPanel GoodsBuyPanelPrefab => _goodsBuyPanelPrefab;
    [SerializeField] private GoodsSellPanel _goodsSellPanelPrefab;
    public GoodsSellPanel GoodsSellPanelPrefab => _goodsSellPanelPrefab;

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
}
