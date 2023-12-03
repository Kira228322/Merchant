using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Npc))]
public class ObjectConversation : MonoBehaviour, IPointerClickHandler
{
    //��������� NPC ��� ���������� ��������, �� ����� NPC �� ���������, �� �����
    //��������������� ������ ��� ������� � �� ���������� FunctionalWindow � ������ ��� �����.
    //��� ������ ������, � ������� ����� ��������

    private Npc _npc;
    private float _distanceToUse = 3.1f;
    
    private void Awake()
    {
        _npc = GetComponent<Npc>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;
        
        DialogueManager.Instance.EnterDialogueMode(_npc);
    }
}
