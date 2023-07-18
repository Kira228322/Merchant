using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Region : MonoBehaviour
{
    [SerializeField] private List<Location> _locations;

    [SerializeField] private List<NpcQuestGiverData> _questGivers = new();
    
    [HideInInspector, SerializeField] private Dictionary<Item.ItemType, float> _coefsForItemTypes = new (); 
    public Dictionary<Item.ItemType, float> CoefsForItemTypes => _coefsForItemTypes;
    
    [HideInInspector]public List<float> tmpFloat; // пока без этого не обойтись. нужен для кастом эдитора
    public Dictionary<string, int[]> ItemEconomyParams = new ();
    [SerializeField] private TextAsset cvsEconomyParams;
    public int AveragePopulation; // параметр нужный для заполенения подобного dictionary в каждой Location данного региона
    [HideInInspector] public Dictionary<string, int> CountOfEachItem;

    public List<Location> Locations => _locations;

    public NpcQuestGiverData GetRandomFreeQuestGiver()
    {
        List<NpcQuestGiverData> availableQuestGivers = 
            _questGivers.Where(questGiver => questGiver.IsReadyToGiveQuest()).ToList();
        
        if (availableQuestGivers.Count == 0) 
            return null;
        
        return availableQuestGivers[UnityEngine.Random.Range(0, availableQuestGivers.Count)];
    }
    
    
    public void FillEconomyParamsDictionary()
    {
        char lineEnding = '\n';
        
        Encoding encoding = Encoding.UTF8;
        byte[] fileBytes = cvsEconomyParams.bytes;
        string fileContent = encoding.GetString(fileBytes);

        string[] rows = fileContent.Split(new[] { lineEnding });
        for (int i = 1; i < rows.Length - 1; i++)
        {
            string[] cells = rows[i].Split(';');
            // Debug.Log(i + " " + cells[0]);
            // TODO посмотреть, нужно ли -1 в цикле или нет 
            ItemEconomyParams.Add(cells[0], new [] 
            {Convert.ToInt32(cells[1]), Convert.ToInt32(cells[2]),Convert.ToInt32(cells[3])});
        }
        FillDictionariesOfLocations();
    }

    public void FillCoefsForItemTypesDictionary()
    {
        int i = 0;
        foreach (var objItemType in Enum.GetValues(typeof(Item.ItemType)))
        {
            Item.ItemType itemType = (Item.ItemType)objItemType;
            CoefsForItemTypes[itemType] = tmpFloat[i];
            i++;
        }
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
        if (result > 1.4f)
            result = 1.4f;
        else if (result < 0.71f) // 1/1.4f
            result = 0.71f;
        return result;
    }

    public int CalculateGainOnMarket(int currentQuantity, int P, int Q, int A, int C, int budget)
    {
        if (C > 0)
            C = -C;
        
        int C1 = A - C - 2 * Q;
        float B = (float)A / (C + Q) - P;
        
        if (currentQuantity < -C)
            currentQuantity = -C + 1;
        else if (currentQuantity > A - C1)
            currentQuantity = A - C1 - 1;

        budget += P / 2;
        budget += Random.Range(-budget/10, budget/10 +1);
        
        int boughtCount = (int)Math.Round(budget / ((float)A / (currentQuantity + C) - B));
        int produceCount = (int)Math.Round(budget / ((float)A / (-currentQuantity + A - C1) - B));

        return produceCount - boughtCount;
    }
}
