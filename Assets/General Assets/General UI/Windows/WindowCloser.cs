using UnityEngine;
using UnityEngine.EventSystems;

public class WindowCloser : MonoBehaviour, IPointerClickHandler
{
    // При нажатии на эти объекты все окна всплывающие будут уничтожаться 
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var window in MapManager.Windows)
        {
            Destroy(window.gameObject);
        }
    }
}
