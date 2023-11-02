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
    private void Awake()
    {
        _npc = GetComponent<Npc>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueManager.Instance.EnterDialogueMode(_npc);
    }
}
