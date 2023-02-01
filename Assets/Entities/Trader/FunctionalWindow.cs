using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FunctionalWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _NPCName;
    [SerializeField] private GameObject _tradeButton;
    private NPC _NPC;

    
    
    public void Init(NPC npc)
    {
        _tradeButton.SetActive(false);
        _NPC = npc;
        
        _NPCName.text = _NPC.Name; 
        if (_NPC is Trader)
            _tradeButton.SetActive(true);
    }
    
    public void OnTalkButtonClick()
    {
        DialogueManager.Instance.EnterDialogueMode(_NPC.InkJSON);
        Destroy(gameObject);
    }

    public void OnTradeButtonClick()
    {
        TradeManager.Singleton.OpenTradeWindow((Trader)_NPC);
        Destroy(gameObject);
    }
    
}
