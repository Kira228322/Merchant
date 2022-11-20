using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private SlidersController _hungerScale;
    [SerializeField] private SlidersController _sleepScale;
    
    public int MaxHunger; // ������������ �������� ������ � ��� ����� ����� ������� ����� ����� ����� � ����
    public int MaxSleep; // "���������" ��� ���� ����.
    public int CurrentHunger;
    public int CurrentSleep; // ���� �� ��� ���� � �������� ���� �����. ����� ���� ����� public

    public void EatFood(int foodValue)
    {
        CurrentHunger += foodValue;
        if (CurrentHunger > MaxHunger)
            CurrentHunger = MaxHunger;
        _hungerScale.SetValue(CurrentHunger, MaxHunger);
    }

    public void Sleep()
    {
        // TODO ����� ������� ���, ����� ���� ����� ���� �������� ����� ��� � �� ������ ��� ��� ���������������
        // 1/8 �� MaxSleep (�� ���� �������� ����� ������� 8 �����, ������ ��� ������) 
    }
    
}
