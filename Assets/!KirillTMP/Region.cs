using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] private List<Location> _locations;

    public Dictionary<Item.ItemType, float> _coefsForItemTypes; // TODO � ��������� ���������� ��������� ����� ��� ��������� ItemType.
    
    // Todo ���� ��������� ��� ������� �������� � ���� ��� ������� �������. 
    // P - avg price
    // Q, A, C - ������. ����� ��� � ����� ������� � excel ��� ���-�� ��������. ��������� ��� ������ ������������ - ���������.

    public int CalculatePrice(int currentQuantity,int P, int Q, int A, int C)
    {
        float B = (float)A / (C + Q) - P;
        return (int)Math.Round((float)A / (currentQuantity + C) - B);
    }

    public int CalculateGainOnMarket(int currentQuantity, int P, int Q, int A, int C, int budget)
    {
        int C1 = A - C - 2 * Q;
        float B = (float)A / (C + Q) - P;
        int boughtCount = (int)Math.Round(budget / ((float)A / (currentQuantity + C) - B));
        int produceCount = (int)Math.Round(budget / ((float)A / (-currentQuantity + A - C1) - B));

        return produceCount - boughtCount;
    }
}
