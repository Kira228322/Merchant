using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    // ��������� ������ (������������ �������� �� ������ � � ����� � �� ���� ������, �� ��������, �� ������������)
    
    [SerializeField] private float _travelingTime; // ���� ������ � ����� �������� ����� ���������� (������� �����
    public float TravelingTime => _travelingTime;  // ��� � �������� ��������/�������) 

    public PlaceOnMap[] Points = new PlaceOnMap[2];
}
