using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectConversation : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextAsset _inkDialogue;

    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueManager.Instance.EnterDialogueMode(_inkDialogue);
    }
}
