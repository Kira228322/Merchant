using System.Collections.Generic;
using UnityEngine;


public abstract class EventInTravel : MonoBehaviour
{
    public string EventName;
    public int Weight;
    [TextArea(2,6)]public string Description;
    [HideInInspector] public List<string> ButtonsLabel; // �������� ���������� ������ + �� �������� 
    public abstract void SetButtons();
    
    public abstract void OnButtonClick(int n);

    protected virtual void SetInteractable() // ����� ��� ���������� ������ ��������, ������� ������ ���������
    {
        // ���� ����� ������, ���� ����� ������ ����������� �� ���������� ������ � ������ ������ ��� ����������� 
    }
}
