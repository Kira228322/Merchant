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
    
    [HideInInspector]public List<float> tmpFloat; // ���� ��� ����� �� ��������. ����� ��� ������ �������
    public Dictionary<string, int[]> ItemEconomyParams = new ();
    [SerializeField] private TextAsset cvsEconomyParams;
    public int AveragePopulation; // �������� ������ ��� ����������� ��������� dictionary � ������ Location ������� �������
    [HideInInspector] public Dictionary<string, int> CountOfEachItem;

    public List<Location> Locations => _locations;

    public NpcQuestGiverData GetRandomFreeQuestGiver()
    {
        List<NpcQuestGiverData> availableQuestGivers =
        _questGivers.Where(questGiver => questGiver.IsReadyToGiveQuest()).ToList();

        //����� ������ ����� ������������, ������� ��� ���� �� �����. �� ������ ����� ����� ����� ������

        //HashSet - �����������, ������������ ����� ���. ��� �������
        HashSet<NpcQuestGiverData> questGiverDatasOnScene = new
        (
            FindObjectsOfType<Npc>()
                .Where(npc => npc.NpcData is NpcQuestGiverData)
                .Select(npc => (NpcQuestGiverData)npc.NpcData)
        );

        availableQuestGivers.RemoveAll(questGiver => questGiverDatasOnScene.Contains(questGiver));

        if (availableQuestGivers.Count == 0)
            return null;

        return availableQuestGivers[Random.Range(0, availableQuestGivers.Count)];
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
            // TODO ����������, ����� �� -1 � ����� ��� ��� 
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

    public void Initialize() //������������� ��� ������ ������� ����
    {
        //��������������� ��� FillDictionary ��� ���������
        CountOfEachItem = new();
        foreach (var item in ItemEconomyParams)
        { // ������������� ������� ����� ���������� � ���� 
            CountOfEachItem.Add(item.Key, item.Value[0]); // item.Value[0] ����������� �����
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
        foreach (var item in ItemEconomyParams)
            CountOfEachItem[item.Key] = 0;
        
        for (int i = 0; i < _locations.Count; i++)
            foreach (var countOfItem in _locations[i].CountOfEachItem)
            {
                CountOfEachItem[countOfItem.Key] += countOfItem.Value;
            }
    }
    

    public float CalculatePriceCoef(int currentQuantity, int P, int Q, int A, int C)
    {
        if (currentQuantity <= C)
            currentQuantity = C + 1;
        
        float B = (float)A / (Q - C) - P;
        float result = (float)Math.Round((float)A / (currentQuantity - C) - B) / P;
        if (result > 1.4f)
            result = 1.4f;
        else if (result < 0.71f) // 1/1.4f
            result = 0.71f;
        return result;
    }

    public int CalculateGainOnMarket(int currentQuantity, int P, int Q, int A, int C, int budget)
    {
        // upd ������ � ���� ����� �������������, ������ �������������. ����� ������� ����� (� ������� ��� ��� �������������, � ������� ���������� �� ������)
        int C1 = A + C - 2 * Q;
        float B = (float)A / (Q - C) - P;
        
        if (currentQuantity <= C)
            currentQuantity = C + 1;
        else if (currentQuantity >= A - C1)
            currentQuantity = A - C1 - 1;

        budget += P / 2 + Random.Range(-budget/10, budget/10 + 1);

        float boughtPrice = (float)A / (currentQuantity - C) - B;
        float producePrice = (float)A / (-currentQuantity + A - C1) - B;

        if (boughtPrice < P / 10f)
            boughtPrice = P / 10f;
        else if (producePrice < P / 10f)
            producePrice = P / 10f;
        
        int boughtCount = (int)Math.Round(budget / boughtPrice);
        int produceCount = (int)Math.Round(budget / producePrice);
        
        if (produceCount == boughtCount) // 10%, ��� ���� ������ �� ����� ����������� �����, �� ��� ������� ��� �������� 
            if (Random.Range(0, 10) == 0) // ��������� � ������������. ����� ��� ���� ������ �����-�� ���� ����� �������
            {
                if (Random.Range(0, 2) == 0)
                    produceCount += Q / 6 + 2;
                else
                    boughtCount += Q / 6 + 2;
        
                if (produceCount - boughtCount < -currentQuantity)
                    return -currentQuantity;
            }


        if (produceCount - boughtCount < -currentQuantity) // ���� ������ ��������� ����� ������, ��� �� ����
            return -currentQuantity + Q; // �� ������ ���� ����� �������, ������� �������� ���������� � Q 
        return produceCount - boughtCount;
    }
}
