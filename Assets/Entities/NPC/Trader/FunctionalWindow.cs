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
        _canvas = FindObjectOfType<CanvasWarningGenerator>().transform;
        _NPC = npc;
        
        _NPCName.text = _NPC.NpcData.Name; 
        // Выставление кнопок:
        if (_NPC is NpcTrader) // Если он трейдер то 2 кнопки (трейд и разговор)
            Destroy(_wagonUpButton);
        else if (!(_NPC is NPCWagonUpgrader)) // Если он не трейдер и не апгрейдер, то он обычный нпс (только поговорить)
        {
            Destroy(_wagonUpButton);
            Destroy(_tradeButton);
        }
        // если 2 ифа не верны, то он апргейдер, у него все 3 кнопки
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
        Instantiate(_wagonUpgradeWindow.gameObject, _canvas).GetComponent<WagonUpgradeWindow>().Init((NPCWagonUpgrader)_NPC);
        TradeManager.Instance.PlayerBlock.alpha = 0;
        Destroy(gameObject);
    }
}
