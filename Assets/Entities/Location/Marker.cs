using UnityEngine;
using UnityEngine.EventSystems;

public class Marker : MonoBehaviour, IPointerClickHandler
{
    private MonoBehaviour _usableObject;

    public void Init(MonoBehaviour obj)
    {
        _usableObject = obj;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_usableObject is UsableEnvironment usableEnvironment)
            usableEnvironment.OnPointerClick(eventData);
        else if (_usableObject is CraftingStation craftingStation)
            craftingStation.OnPointerClick(eventData);
        else if (_usableObject is Noticeboard noticeboard)
            noticeboard.OnPointerClick(eventData);
        else if (_usableObject is ObjectConversation objectConversation)
            objectConversation.OnPointerClick(eventData);
    }
}
