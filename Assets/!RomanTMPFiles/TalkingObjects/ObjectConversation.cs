using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Npc))]
public class ObjectConversation : MonoBehaviour, IPointerClickHandler
{
    //—имул€ци€ NPC дл€ проведени€ диалогов, но такие NPC не двигаютс€, не имеют
    //восклицательных знаков над головой и не отображают FunctionalWindow с именем при клике.
    //Ёто просто объект, с которым можно говорить

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
