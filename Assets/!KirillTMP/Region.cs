using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] private List<Location> _locations;

    [HideInInspector] public Dictionary<Item.ItemType, float> _coefsForItemTypes; // TODO в кастомном инспекторе заполнить сразу все возможные ItemType.
    
    [HideInInspector]public List<float> tmp; // пока без этого не обойтись. нужен для кастом эдитора
    public Dictionary<string, int[]> ItemEconomyParams = new ();
    [SerializeField] private TextAsset cvsEconomyParams;
    public int AveragePopulation; // параметр нужный для заполенения подобного dictionary в каждой Location данного региона
    [HideInInspector] public Dictionary<string, int> CountOfEachItem;

    public List<Location> Locations => _locations;

    public void FillDictionary()
    {
        char lineEnding = '\n';
        string[] rows = cvsEconomyParams.text.Split(lineEnding);
        for (int i = 1; i < rows.Length - 1; i++)
        {
            string[] cells = rows[i].Split(';');
            ItemEconomyParams.Add(cells[0], new [] 
            {Convert.ToInt32(cells[1]), Convert.ToInt32(cells[2]),Convert.ToInt32(cells[3])});
        }
        FillDictionariesOfLocations();
    }

    public void Initialize() //Инициализация при первом запуске игры
    {
        //Подразумевается что FillDictionary уже произошёл
        CountOfEachItem = new();
        foreach (var item in ItemEconomyParams)
        { // инициализация словаря всеми предметами в игре 
            CountOfEachItem.Add(item.Key, item.Value[0]); // item.Value[0] равновесное число
        }
        InitializeLocations();
    }
    private void InitializeLocations()
    {
        for (int i = 0; i < _locations.Count; i++)
        {
            _locations[i].Initialize();
        }
    }
    private void FillDictionariesOfLocations()
    {
        for (int i = 0; i < _locations.Count; i++)
        {
            _locations[i].FillDictionary();
        }
    }
    
    public void CountAllItemsInRegion()
    {
        for (int i = 0; i < _locations.Count; i++)
            foreach (var countOfItem in _locations[i].CountOfEachItem)
                CountOfEachItem[countOfItem.Key] += countOfItem.Value;
    }
    

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
