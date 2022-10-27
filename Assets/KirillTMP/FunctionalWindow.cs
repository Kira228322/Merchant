using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FunctionalWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _merchantName;
    private Merchant _merchant;

    
    
    public void Init(Merchant merchant)
    {
        _merchant = merchant;
        
        _merchantName.text = _merchant.Name; 
    }
    
    public void OnTalkButtonClick()
    {
        
    }

    public void OnBuyButtonClick()
    {
        
    }

    public void OnSellButtonClick()
    {
        
    }
}
