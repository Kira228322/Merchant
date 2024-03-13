using UnityEngine;
using UnityEngine.EventSystems;

public class WindowCloser : MonoBehaviour, IPointerClickHandler
{
    // ��� ������� �� ��� ������� ��� ���� ����������� ����� ������������ 
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var window in MapManager.Windows)
        {
            Destroy(window.gameObject);
        }
    }
}
