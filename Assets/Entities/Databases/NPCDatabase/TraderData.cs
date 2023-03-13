using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newTraderData", menuName = "NPCs/TraderData")]
public class TraderData : NPCData
{
    public List<TraderType> TraderTypes;
    public int RestockCycle;
    public int LastRestock;
    [HideInInspector] public List<NPCTrader.TraderGood> Goods;

    
    [CustomEditor(typeof(TraderData))]
    class EditorTrader : Editor
    {
        private TraderData _traderData;
        private void OnEnable()
        {
            _traderData = (TraderData)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(20);
            if (GUILayout.Button("Добавить товар"))
                _traderData.Goods.Add(new NPCTrader.TraderGood());

            if (_traderData.Goods.Count > 0)
            {
                foreach (var good in _traderData.Goods)
                {
                    GUILayout.Space(-1);
                    EditorGUILayout.BeginVertical("box");

                    
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                    {
                        _traderData.Goods.Remove(good);
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
                    GUILayout.Label("Max count of this good", GUILayout.MaxWidth(136));
                    good.MaxCount = EditorGUILayout.IntField(good.MaxCount,  GUILayout.MaxWidth(50));
                    good.Count = good.MaxCount;
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Price", GUILayout.MaxWidth(40));
                    good.CurrentPrice = EditorGUILayout.IntField(good.CurrentPrice,  GUILayout.MaxWidth(50));
                    GUILayout.Space(36);
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
