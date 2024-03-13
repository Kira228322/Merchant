using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public event UnityAction<bool> IsButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsButtonPressed?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsButtonPressed?.Invoke(false);
    }
}
