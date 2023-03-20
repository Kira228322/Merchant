using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newTraderData", menuName = "NPCs/TraderData")]
public class NpcTraderData : NPCData, IResetOnExitPlaymode, ISaveable<NpcTraderSaveData>
{
    public List<TraderType> TraderTypes;
    public int RestockCycle;

    [HideInInspector, SerializeField] private List<NPCTrader.TraderGood> _baseGoods = new ();
    [HideInInspector, SerializeField] private List<NPCTrader.TraderGood> _baseAdditiveGoods = new ();
    [HideInInspector, SerializeField] private List<NPCTrader.BuyCoefficient> _baseBuyCoefficients = new ();

    public int LastRestock;

    [HideInInspector] public List<NPCTrader.TraderGood> Goods;
    [HideInInspector] public List<NPCTrader.TraderGood> AdditiveGoods;
    [HideInInspector] public List<NPCTrader.BuyCoefficient> BuyCoefficients;

    NpcTraderSaveData ISaveable<NpcTraderSaveData>.SaveData()
    {
        return new(Affinity, ID, LastRestock, Goods, AdditiveGoods, BuyCoefficients);
    }

    void ISaveable<NpcTraderSaveData>.LoadData(NpcTraderSaveData data)
    {
        Affinity = data.Affinity;
        LastRestock = data.LastRestock;

        List<NPCTrader.TraderGood> newGoods = new();
        foreach (var savedTraderGood in data.Goods)
        {
            newGoods.Add(new(savedTraderGood));
        }
        Goods = newGoods.Select(good => new NPCTrader.TraderGood(good)).ToList();

        List<NPCTrader.TraderGood> newAdditiveGoods = new();
        foreach (var savedTraderGood in data.AdditiveGoods)
        {
            newAdditiveGoods.Add(new(savedTraderGood));
        }
        AdditiveGoods = newAdditiveGoods.Select(good => new NPCTrader.TraderGood(good)).ToList();

        List<NPCTrader.BuyCoefficient> newBuyCoefficients = new();
        foreach (var buyCoefficitent in data.BuyCoefficients)
        {
            newBuyCoefficients.Add(buyCoefficitent);
        }
        BuyCoefficients = newBuyCoefficients.Select(buyCoefficient => new NPCTrader.BuyCoefficient(buyCoefficient)).ToList();
    }

    private void OnEnable()
    {
        Affinity = _baseAffinity;
        Goods = _baseGoods.Select(good => new NPCTrader.TraderGood(good)).ToList();
        AdditiveGoods = _baseAdditiveGoods.Select(good => new NPCTrader.TraderGood(good)).ToList();
        SetBuyCoefficients();
        BuyCoefficients = _baseBuyCoefficients.Select(buyCoefficient => new NPCTrader.BuyCoefficient(buyCoefficient)).ToList();
    }
    void IResetOnExitPlaymode.ResetOnExitPlaymode()
    {
        Affinity = _baseAffinity;
        Goods = _baseGoods;
        AdditiveGoods = _baseAdditiveGoods;
        BuyCoefficients = _baseBuyCoefficients;
    }

    private void SetBuyCoefficients()
    {
        _baseBuyCoefficients.Clear();
        Dictionary<Item.ItemType, TraderType.TraderGoodType> mergedTraderGoods = new();

        foreach(TraderType traderType in TraderTypes)
        {
            foreach(TraderType.TraderGoodType traderGoodType in traderType.TraderGoodTypes)
            {
                if (!mergedTraderGoods.ContainsKey(traderGoodType.ItemType))
                {
                    // If the ItemType is not in the dictionary, add a new key-value pair with a copy of the current TraderGoodType
                    mergedTraderGoods.Add(traderGoodType.ItemType, new TraderType.TraderGoodType(traderGoodType));
                }
                else
                {
                    // If the ItemType is already in the dictionary, update the values of the corresponding TraderGoodType
                    TraderType.TraderGoodType mergedTraderGoodType = mergedTraderGoods[traderGoodType.ItemType];

                    if (traderGoodType.Coefficient != 1 && mergedTraderGoodType.Coefficient != 1)
                    {
                        mergedTraderGoodType.Coefficient += traderGoodType.Coefficient;
                        mergedTraderGoodType.Coefficient /= 2;

                        mergedTraderGoodType.CountToBuy += traderGoodType.CountToBuy;
                        mergedTraderGoodType.CountToBuy /= 2;
                        //����� ����� � ��������������, ��� � �������� ������������ �� ����� ����� ���� ����� TraderType =>
                        //������ ������� �� 2, ���� ���� �������� ������� �������� (20.03.23)
                    }
                    else //TODO: ���� else �������� �� ������ �����: ���� � ������� TraderType � ����� ��� ����������� 1 � CountToBuy 50,
                         //� � ������� ����������� 0.8 � CountToBuy 30 �� �������� ����� Coefficient 1 & CountToBuy 30 (��������� 50)
                         //� ����� ������� �� ������� ���������� � ����� TraderType. � ������� ����� ����� �������� �������� (20.03.23)
                    {
                        mergedTraderGoodType.Coefficient = 1;
                        mergedTraderGoodType.CountToBuy = traderGoodType.CountToBuy;
                    }
                }
            }
        }

        foreach (var traderGoodType in mergedTraderGoods)
        {
            _baseBuyCoefficients.Add(new(traderGoodType.Value));
        }

        /*
        foreach (Item.ItemType itemType in Enum.GetValues(typeof(Item.ItemType)))
        //Enum.GetValues(typeof(Item.ItemType)) ���������� ��� ��������� �������� Item.ItemType
        //(���� ����� ������� ����� ItemType, ���� �� �������� ������������)
        {
            NPCTrader.BuyCoefficient traderBuyCoefficient = new();
            traderBuyCoefficient.itemType = itemType;
            traderBuyCoefficient.Coefficient = 0;
            traderBuyCoefficient.CountToBuy = 0;
            //���� �� �� ������������ (�.� �� ������ ����� �� ����� ������� ����� ����������� ����� ���������, �� ��� ��������� 0, ��� � ����.)
            _baseBuyCoefficients.Add(traderBuyCoefficient);
        }
        //����� ������� �� ������� ������� BuyCoefficient, ������� � ��� ����� ���������� Item.ItemType.
        //������ ����� ������ ������� �������� ���� �������� �������������
        foreach (NPCTrader.BuyCoefficient traderBuyCoefficient in _baseBuyCoefficients)
        {

            //����� ��� traderGoodType, � ������� ������������ ����� �� itemType:
            List<TraderType.TraderGoodType> acceptableTraderGoodTypes =
            TraderTypes.SelectMany(traderType => traderType.TraderGoodTypes.Where
            (traderGoodType => traderGoodType.ItemType == traderBuyCoefficient.itemType)).ToList();
            //���� Linq ���������� ��� TraderGoodType, �������
            //����� ����� �� ItemType ��� � ������ ���������������� traderBuyCoefficient
            
            foreach (TraderType.TraderGoodType traderGoodType in acceptableTraderGoodTypes)
            {
                traderBuyCoefficient.CountToBuy += traderGoodType.CountToBuy;
            }
            foreach (TraderType.TraderGoodType traderGoodType in acceptableTraderGoodTypes)
            {
                if (traderGoodType.Coefficient == 1)
                {
                    traderBuyCoefficient.Coefficient = 1;
                    continue;
                }
                traderBuyCoefficient.Coefficient += traderGoodType.Coefficient;
            }
            if (acceptableTraderGoodTypes.Count != 0) //�������� ������� �� ����, ����� ���� ����� ����� Coefficient & CountToBuy == 0
            {
                traderBuyCoefficient.CountToBuy = (int)Math.Ceiling((float)traderBuyCoefficient.CountToBuy / acceptableTraderGoodTypes.Count);
                traderBuyCoefficient.DefaultCountToBuy = traderBuyCoefficient.CountToBuy;
                if (traderBuyCoefficient.Coefficient != 1)
                    traderBuyCoefficient.Coefficient = (float)Math.Round(traderBuyCoefficient.Coefficient / acceptableTraderGoodTypes.Count, 2);
            }
        }
        foreach (NPCTrader.BuyCoefficient buyCoefficient in _baseBuyCoefficients)
        {
            Debug.Log(buyCoefficient.itemType.ToString() + buyCoefficient.CountToBuy);
        }
        */
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
            if (GUILayout.Button("�������� �����"))
                _traderData._baseGoods.Add(new NPCTrader.TraderGood());

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
