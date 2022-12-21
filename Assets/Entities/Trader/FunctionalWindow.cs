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
        TradeManager.Singleton.OpenTradeWindow(_trader);
        Destroy(gameObject);
    }
    
}
