using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class EventInTravel : MonoBehaviour
{
    public string EventName;
    [TextArea(2,6)]public string Description;
    public List<string> ButtonsLabel; // �������� ����������� ������ + �� �������� 

    public abstract void OnButtonClick();
    
    // ���� ������, ������ ��� ���� �����
    // ������ ��� ������� ����������� ������� ����� ����� ������, ������� ����������� �� EventInTravel
    // ������ ���� �������� ��� ��������� ���������� ������ ������ � ������� switch �� ������ ������� ������
    // �������� �������� ������� ��������� ������ ��� ������ �� ����� ������� � ������
    // ��� �� ����� ����� ������ ��������� ����� ���� ����� ������
    // ���� ���� ������ + ������, ����� ��� ��� ������, ��� ����� ���-�� ��������, ������ �� ��� ����� 
    
}
