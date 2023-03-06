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
        // ����������� ������:
        if (_NPC is NPCTrader) // ���� �� ������� �� 2 ������ (����� � ��������)
            Destroy(_wagonUpButton);
        else if (!(_NPC is NPCWagonUpgrader)) // ���� �� �� ������� � �� ���������, �� �� ������� ��� (������ ����������)
        {
            Destroy(_wagonUpButton);
            Destroy(_tradeButton);
        }
        // ���� 2 ��� �� �����, �� �� ���������, � ���� ��� 3 ������
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
