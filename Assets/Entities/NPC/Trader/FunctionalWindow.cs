using System;
using TMPro;
using UnityEngine;

public class FunctionalWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _NPCName;
    [SerializeField] private GameObject _tradeButton;
    [SerializeField] private GameObject _wagonUpButton;
    [SerializeField] private WagonUpgradeWindow _wagonUpgradeWindow;
    private Npc _NPC;
    private Transform _canvas;

    
    public void Init(Npc npc)
    {
        _canvas = CanvasWarningGenerator.Instance.transform;
        _NPC = npc;
        
        _NPC.StartInteraction();
        
        _NPCName.text = _NPC.NpcData.Name;

        if (_NPC.NpcData.ID >= 9000) // не квестовый нпс
            _NPCName.color = new Color(58f / 255, 58f / 255, 58f / 255, 1);
        
        switch (_NPC)
        {
            case NpcTrader:
                Destroy(_wagonUpButton);
                break;
            case NpcWagonUpgrader:
                Destroy(_tradeButton);
                break;
            default:
                Destroy(_tradeButton);
                Destroy(_wagonUpButton);
                break;
        }
    }
    
    public void OnTalkButtonClick()
    {
        DialogueManager.Instance.EnterDialogueMode(_NPC);
        Destroy(gameObject);
    }

    public void OnTradeButtonClick()
    {
        NpcTrader trader = (NpcTrader)_NPC;
        trader.OpenTradeWindow();
        
        Destroy(gameObject);
    }

    public void OnWagonUpgradeButtonClick()
    {
        Instantiate(_wagonUpgradeWindow.gameObject, _canvas).GetComponent<WagonUpgradeWindow>().Init((NpcWagonUpgrader)_NPC);
        TradeManager.Instance.PlayerBlock.alpha = 0;
        Destroy(gameObject);
    }

    public void OnCloseButtonClick()
    {
        _NPC.StopInteraction();
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameManager.Instance.CurrentFunctionalWindow = gameObject;
    }

    private void OnDisable()
    {
        GameManager.Instance.CurrentFunctionalWindow = null;
    }
}
