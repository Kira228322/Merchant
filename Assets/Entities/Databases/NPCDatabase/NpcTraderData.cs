using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newTraderData", menuName = "NPCs/TraderData")]
public class NpcTraderData : NpcData, IResetOnExitPlaymode, ISaveable<NpcTraderSaveData>
{
    public List<TraderType> TraderTypes;
    public int RestockCycle;

    [HideInInspector, SerializeField] private List<NpcTrader.TraderGood> _baseGoods = new ();
    [HideInInspector, SerializeField] private List<NpcTrader.BuyCoefficient> _baseBuyCoefficients = new ();
    //AdditiveGoods рассчитываются только в рантайме или при загрузке, с инспектором никак не связаны

    public int LastRestock;

    [HideInInspector] public List<NpcTrader.TraderGood> Goods;
    [HideInInspector] public List<NpcTrader.TraderGood> AdditiveGoods;
    [HideInInspector] public List<NpcTrader.BuyCoefficient> BuyCoefficients;

    NpcTraderSaveData ISaveable<NpcTraderSaveData>.SaveData()
    {
        return new(Affinity, ID, LastRestock, Goods, AdditiveGoods, BuyCoefficients);
    }

    void ISaveable<NpcTraderSaveData>.LoadData(NpcTraderSaveData data)
    {
        Affinity = data.Affinity;
        LastRestock = data.LastRestock;

        Goods = data.Goods.Select(good => new NpcTrader.TraderGood(good)).ToList();

        AdditiveGoods = data.AdditiveGoods.Select(good => new NpcTrader.TraderGood(good)).ToList();

        BuyCoefficients = data.BuyCoefficients.Select(buyCoefficient => new NpcTrader.BuyCoefficient(buyCoefficient)).ToList();
    }

    private void OnEnable()
    {
        Affinity = _baseAffinity;
        Goods = _baseGoods.Select(good => new NpcTrader.TraderGood(good)).ToList();
        //AdditiveGoods рассчитываются только в рантайме или при загрузке сохранения, с инспектором никак не связаны
        SetBuyCoefficients();
        BuyCoefficients = _baseBuyCoefficients.Select(buyCoefficient => new NpcTrader.BuyCoefficient(buyCoefficient)).ToList();
    }
    void IResetOnExitPlaymode.ResetOnExitPlaymode()
    {
        Affinity = _baseAffinity;
        Goods.Clear();
        AdditiveGoods.Clear();
        BuyCoefficients.Clear();
    }

    private void SetBuyCoefficients()
    {
        _baseBuyCoefficients.Clear();
        Dictionary<Item.ItemType, TraderType.TraderGoodType> mergedTraderGoods = new();

        foreach (var traderGoodType in TraderTypes.SelectMany(tt => tt.TraderGoodTypes))
        {
            // Если в словаре уже есть такой ItemType, то обновляет его значения
            // (считает средние между старым и новым или приравнивает к единице)
            // Если ещё нету, то добавляет новый TraderGoodType с этим ItemType
            mergedTraderGoods[traderGoodType.ItemType] = mergedTraderGoods.TryGetValue(traderGoodType.ItemType, out var existingTraderGoodType)
                ? new TraderType.TraderGoodType
                {
                    ItemType = traderGoodType.ItemType,
                    Coefficient = traderGoodType.Coefficient != 1 && existingTraderGoodType.Coefficient != 1
                    ? (existingTraderGoodType.Coefficient + traderGoodType.Coefficient) / 2
                    : 1,
                    CountToBuy = traderGoodType.Coefficient != 1 && existingTraderGoodType.Coefficient != 1
                    ? (existingTraderGoodType.CountToBuy + traderGoodType.CountToBuy) / 2
                    : Math.Max(existingTraderGoodType.CountToBuy, traderGoodType.CountToBuy)
                }
                : new(traderGoodType);
        }

        List<NpcTrader.BuyCoefficient> resultingBuyCoefficients = mergedTraderGoods.Values.Select(type => new NpcTrader.BuyCoefficient(type)).ToList();
        _baseBuyCoefficients.AddRange(resultingBuyCoefficients);

    }

    [CustomEditor(typeof(NpcTraderData))]
    class EditorTrader : Editor
    {
        private NpcTraderData _traderData;
        private void OnEnable()
        {
            _traderData = (NpcTraderData)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(20);
            if (GUILayout.Button("Добавить товар"))
                _traderData._baseGoods.Add(new NpcTrader.TraderGood());

            if (_traderData._baseGoods.Count > 0)
            {
                foreach (var good in _traderData._baseGoods)
                {
                    GUILayout.Space(-1);
                    EditorGUILayout.BeginVertical("box");

                    
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                    {
                        _traderData._baseGoods.Remove(good);
                        break;
                    }
                    EditorGUILayout.Space(5);
                    GUILayout.Label("Item", GUILayout.MaxWidth(30));
                    good.Good = (Item)EditorGUILayout.ObjectField(good.Good,
                        typeof(Item), false,  GUILayout.MaxWidth(156));
                    GUILayout.FlexibleSpace();
                    if (good.Good != null)
                        EditorGUILayout.LabelField("Average Price - " + good.Good.Price, GUILayout.MaxWidth(130));
                    EditorGUILayout.EndHorizontal();
                    
                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space(27);
                    GUILayout.Label("Max count", GUILayout.MaxWidth(136));
                    good.MaxCount = EditorGUILayout.IntField(good.MaxCount,  GUILayout.MaxWidth(50));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Current count", GUILayout.MaxWidth(136));
                    good.CurrentCount = EditorGUILayout.IntField(good.CurrentCount, GUILayout.MaxWidth(50));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Price", GUILayout.MaxWidth(40));
                    good.CurrentPrice = EditorGUILayout.IntField(good.CurrentPrice,  GUILayout.MaxWidth(50));
                    GUILayout.Space(27);
                    EditorGUILayout.EndHorizontal();
                    
                    GUILayout.Space(-5);
                    EditorGUILayout.EndVertical();
                }
            }
            
            if (GUI.changed)
                EditorUtility.SetDirty(_traderData);
        }
    }
}
