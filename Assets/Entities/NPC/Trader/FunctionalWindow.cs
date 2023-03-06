using TMPro;
using UnityEngine;

public class FunctionalWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _NPCName;
    [SerializeField] private GameObject _tradeButton;
    [SerializeField] private GameObject _wagonUpButton;
    private NPC _NPC;

    
    
    public void Init(NPC npc)
    {
        _NPC = npc;
        
        _NPCName.text = _NPC.Name; 
        // Выставление кнопок:
        if (_NPC is NPCTrader) // Если он трейдер то 2 кнопки (трейд и разговор)
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
        TradeManager.Instance.OpenTradeWindow((NPCTrader)_NPC);
        Destroy(gameObject);
    }

    public void OnWagonUpgradeButtonClick()
    {
        // TODO
    }
}
