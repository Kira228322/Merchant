using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] private List<Location> _locations;

    public Dictionary<Item.ItemType, float> _coefsForItemTypes; // TODO в кастомном инспекторе заполнить сразу все возможные ItemType.
    
    // TODO
    // public Dictionary<string (ItemName), List<int (Q;A;C)>> 
    // public int AveragePopulation // параметр нужный для заполенения подобного dictionary в каждой Location данного региона
    public float CalculatePriceCoef(int currentQuantity, int P, int Q, int A, int C)
    {
        if (C > 0)
            C = -C;
        
        float B = (float)A / (C + Q) - P;
        float result = (float)Math.Round((float)A / (currentQuantity + C) - B) / P;
        if (result > 1.5f)
            result = 1.5f;
        else if (result < 0.667f) // 1/1.5f
            result = 0.667f;
        return result;
    }

    public int CalculateGainOnMarket(int currentQuantity, int P, int Q, int A, int C, int budget)
    {
        if (C > 0)
            C = -C;
        
        int C1 = A - C - 2 * Q;
        float B = (float)A / (C + Q) - P;
        
        if (currentQuantity < -C)
            currentQuantity = -C;
        else if (currentQuantity > A - C1)
            currentQuantity = A - C1;
        
        int boughtCount = (int)Math.Round(budget / ((float)A / (currentQuantity + C) - B));
        int produceCount = (int)Math.Round(budget / ((float)A / (-currentQuantity + A - C1) - B));

        return produceCount - boughtCount;
    }
}
