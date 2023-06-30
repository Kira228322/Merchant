using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Location : MonoBehaviour
{
    [Header("Village information")]
    [SerializeField] private Region _region;

    public Region Region => _region;
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;
    [SerializeField] private string _villageName;
    public string VillageName => _villageName;
    [SerializeField] [TextArea(2,4)] private string _description;
    public string Description => _description;
    
    [Space(12)]
    [Header("Economy")]
    [HideInInspector] public Dictionary<string , int[]> ItemEconomyParams = new (); // основан на таком же dictionary своего региона Dictionary<string (ItemName), List<int (Q;A;C)>>
    [SerializeField] private int _populationOfVillage; // будет корректировать dictionary сверху, зававая параметры Q и C 
    [HideInInspector] public Dictionary<string, int> CountOfEachItem;
    private int _lastRestockDay;
    
    [Space(12)]
    [Header("Initialization")]

    [SerializeField]private string _sceneName;
    public string SceneName => _sceneName;
    [SerializeField] private List<Location> _relatedPlaces;
    // места в которые можно попасть из данного места (как ребра в графе) 
    public List<Location> RelatedPlaces => _relatedPlaces;
    [HideInInspector] public List<Road> _roads = new();
    
    
    private void Start()
    {
        Road[] roads = FindObjectsOfType<Road>();
        foreach (var road in roads)
        {
            if (road.Points[0] == this)
            {
                _roads.Add(road);
                continue;
            }
            if (road.Points[1] == this)
                _roads.Add(road);
        }
        
    }

    public void FillDictionary()
    {
        //Здесь полагается, что в регионе уже подсосан словарь из файла.
        //Поэтому этот метод вызывается самим регионом, а не в Start
        float coef = 1.1f * _populationOfVillage / _region.AveragePopulation;
        foreach (var EconomyParam in _region.ItemEconomyParams)
        {
            ItemEconomyParams.Add(EconomyParam.Key, new[]
                    {Convert.ToInt32(Math.Round(EconomyParam.Value[0] * coef)),
                    EconomyParam.Value[1],
                    Convert.ToInt32(Math.Round(EconomyParam.Value[2] * coef))});
        }
    }

    public void Initialize() //Вызывается только при начале новой игры, создает словарь
    {
        //Подразумевается, что FillDictionary уже был вызван ранее   
        CountOfEachItem = new();
        foreach (var item in ItemEconomyParams)
        { // инициализация словаря всеми предметами в игре 
            CountOfEachItem.Add(item.Key, item.Value[0]); // item.Value[0] равновесное число
        }

    }

    public void CountAllItemsOnScene()
    {
        NpcTrader[] traders = FindObjectsOfType<NpcTrader>(); // берем всех трейдеров на локе 
        
        foreach (var item in ItemEconomyParams)
            CountOfEachItem[item.Key] = 0; // занулили

        for (int i = 0; i < traders.Length; i++) // посчитали сколько чего
        {
            for (int j = 0; j < traders[i].Goods.Count; j++)
            {
                CountOfEachItem[traders[i].Goods[j].Good.Name] += traders[i].Goods[j].CurrentCount;
            }

            for (int j = 0; j < traders[i].AdditiveGoods.Count; j++)
            {
                CountOfEachItem[traders[i].AdditiveGoods[j].Good.Name] += traders[i].AdditiveGoods[j].CurrentCount;
            }
            
        }
            
    }

    public void OnPlaceClick()
    {
        GameObject win = Instantiate(MapManager.VillageWindow, MapManager.Canvas.transform);
        win.GetComponent<VillageWindow>().Init(this);
    }

    public void Restock()
    {
        if (GameTime.CurrentDay < _lastRestockDay + 2)
            return;
        
        NpcTrader[] traders = FindObjectsOfType<NpcTrader>();
        RestockBuyCoefficients(traders);
        RestockMainGoods(traders);
        AddAdditiveGoods(traders);
        CountAllItemsOnScene();
        _lastRestockDay = GameTime.CurrentDay;
    }

    public void OnEnterOnLocation()
    {
        GameTime.HourChanged += CheckRestock;
    }

    public void OnLeaveLocation()
    {
        GameTime.HourChanged -= CheckRestock;
    }

    private void CheckRestock()
    {
        if (GameTime.Hours == 1)
            if (_lastRestockDay + 2 < GameTime.CurrentDay)
            {
                Restock();
            }
    }
    private void RestockBuyCoefficients(NpcTrader[] traders)
    {
        foreach (var trader in traders)
            trader.RestockCoefficients();
    }

    private void AddAdditiveGoods(NpcTrader[] traders)
    {
        foreach (var trader in traders)
        {
            if (trader.AdditiveGoods.Count > 9)
                trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));
            if (trader.AdditiveGoods.Count > 7)
                trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));
            if (trader.AdditiveGoods.Count > 4)
                trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));

            if (Random.Range(0,3) == 0)
                return;
            
            int count = Random.Range(1, Player.Instance.Statistics.TotalDiplomacy / 3 + 2); // за каждые 3 дипломатии шанс на +1
            // дополнительную шмотку у торговца
            for (int i = 0; i < count; i++)
            {
                NpcTrader.BuyCoefficient traderBuyCoefficient;
                bool isMainGood;
                Item newItem;
                bool reallyNew;

                if (Random.Range(0, 3) == 0)
                    isMainGood = false; // не мейн тип шмотки торговца 
                else
                    isMainGood = true; // мейн тип шмотки торговца

                while (true)
                {
                    reallyNew = true;
                    traderBuyCoefficient = trader.BuyCoefficients[Random.Range(0, trader.BuyCoefficients.Count)];
                    // у мейн шмоток коэф 1
                    if ((traderBuyCoefficient.Coefficient == 1) == isMainGood)
                    {
                        newItem = ItemDatabase.GetRandomItemOfThisType(traderBuyCoefficient.itemType);

                        foreach (var goods in trader.Goods)
                        {
                            if (goods.Good.Name == newItem.Name)
                            {
                                reallyNew = false;
                                break;
                            }
                        }

                        if (!reallyNew)
                            continue;


                        int randomCount = Random.Range(1, 3);
                        
                        if (newItem.Price < 50)
                            randomCount++;
                        
                        NpcTrader.TraderGood newGood = new NpcTrader.TraderGood(newItem.Name, randomCount, 
                            randomCount, newItem.Price + Random.Range(1, newItem.Price/12 + 2));

                        trader.AdditiveGoods.Add(newGood);
                        break;
                    }
                }
            }
        }
    }

    private void RestockMainGoods(NpcTrader[] traders)
    {
        List<NpcTrader> activeTraders = new List<NpcTrader>(); // трейдеры которые будут учавствовать в очередной итерации foreach
        int gainCount; 
        
        foreach (var item in ItemDatabase.Instance.Items.ItemList)
        {
            // считаем сколько должно прирасти таких предметов на локации
            // budget (последний параметр метода) основывается на населении локации и на интересе в этом типе предмета локацией (CoefsForItemTypes)
            // чем интерес в этом предмете больше значит тем он редкостнее и его прирастает меньше.
            gainCount = _region.CalculateGainOnMarket(CountOfEachItem[item.Name], item.Price,
                ItemEconomyParams[item.Name][0], ItemEconomyParams[item.Name][1],
                ItemEconomyParams[item.Name][2], (int)(_populationOfVillage * (2 - _region.CoefsForItemTypes[item.TypeOfItem])));
            
            if (gainCount == 0)
                continue;

            activeTraders.Clear();
            foreach (var trader in traders) // из всех трейдеров выбирамем тех, кто торгует таким товаром
            foreach (var traderGood in trader.Goods)
                if (item.Name == traderGood.Good.Name)
                {
                    activeTraders.Add(trader);
                    break;
                }
            
            activeTraders.Shuffle(); // Благодаря бездушной машине сделал расширения для List 
            // этот метод перемешивает мместами элементы в листе в случайном порядке
            // Для чего это нужно? чтобы ресток не начинался с одного и того же трейдера каждый раз 

            
            
            if (gainCount > 0) // прирост товара
            {
                if (activeTraders.Count == 0)
                {
                    // Если на локации вообще нет торговцев, которые торгует этим 
                    // то предмет рестокнется с 25% шансом и только на половину от необходимого числа.
                    if (Random.Range(0, 4) == 0)
                    {
                        gainCount /= 2;
                        if (gainCount > 0)
                            traders[Random.Range(0,traders.Length)].AdditiveGoods.Add(new NpcTrader.TraderGood(item.Name, 
                                gainCount, gainCount, item.Price + Random.Range(1, item.Price/10 + 2)));
                    }
                }
                
                else
                {
                    // 5 раз прохожусь по всем трейдерам у которых есть этот предмет. Определнное число, так как если делать это
                    // через while(gainCount > 0), то может возникнуть ситуация когда gainCount > 0 и у всех трейдеров
                    // CurrentCount будет = MaxCount и комп возрвется, чтобы этого избежать пришлось бы добавлять дополнительную
                    // переменную для проверки и саму проверку. Мне кажется экономнее будет делать именно через for 5
                    for (int i = 0; i < 5; i++) 
                    {
                        foreach (var trader in activeTraders)
                        {
                            NpcTrader.TraderGood traderGood =
                                trader.Goods.FirstOrDefault(good => good.Good.Name == item.Name);
                            if (traderGood.CurrentCountIsLessMaxCount())
                            {
                                traderGood.CurrentCount++;
                                gainCount--;
                            }
                            if (gainCount == 0)
                                break;
                        }
                        
                        if (gainCount == 0)
                            break;
                    }

                    while (gainCount > 0) // если после этого еще надо добавить предметов то добавляем их уже сверх Max 
                    {
                        foreach (var trader in activeTraders)
                        {
                            NpcTrader.TraderGood traderGood =
                                trader.Goods.FirstOrDefault(good => good.Good.Name == item.Name);
                            
                            traderGood.CurrentCount++;
                            gainCount--;
                            
                            if (gainCount == 0)
                                break;
                        }
                    }
                }
            }
            
            else // убывание товара
            {
                foreach (var trader in traders) 
                foreach (var traderGood in trader.AdditiveGoods) // смотрим у кого в дополнительных есть товар
                    if (item.Name == traderGood.Good.Name)
                    {
                        activeTraders.Add(trader);
                        break;
                    }
                
                // а вот тут по идеи не может случиться такой ситуации, когда gaincount != 0 
                // а предметы у торговцев закончились, так как gainCount всегда толкает числа к равновесному числу.
                while (gainCount != 0)
                {
                    foreach (var trader in activeTraders)
                    {
                        NpcTrader.TraderGood traderGood =
                            trader.AdditiveGoods.FirstOrDefault(good => good.Good.Name == item.Name);
                        if (traderGood.CurrentCount > 0)
                        {
                            traderGood.CurrentCount--;
                            gainCount++;
                            trader.NpcData.CurrentMoney += traderGood.CurrentPrice;
                        }
                        if (gainCount == 0)
                            break;
                    }
                }
                
            }
        }
    }
    
    
}
