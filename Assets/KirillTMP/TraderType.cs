using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "newTraderType", menuName = "Trader")]
public class TraderType : ScriptableObject
{
    [HideInInspector] public List<Item.ItemType> GoodsType;
    [HideInInspector] public List<float> Coefficient; // коэф * стоимость предмета, за которую купит товар этот торговец
    [HideInInspector] public List<int> CountToBuy; // максимальное число предметов данного типа, которое готов купить продавец
        // будет обновляться с restorom 
    

    [CustomEditor(typeof(TraderType))]
    public class TraderTypeEditor : Editor
    {
        private int n = -1;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            TraderType traderType = (TraderType)target;
            
            
            for (int i = 0; i < 100; i++)
            {
                if (Enum.IsDefined(typeof(Item.ItemType), i))
                {
                    traderType.GoodsType.Add((Item.ItemType)i);
                    traderType.Coefficient.Add(0);
                    traderType.CountToBuy.Add(0);
                }
                else
                {
                    n = i;
                    while (traderType.GoodsType.Count > n)
                    {
                        traderType.GoodsType.RemoveAt(traderType.GoodsType.Count -1);
                        traderType.Coefficient.RemoveAt(traderType.Coefficient.Count - 1);
                        traderType.CountToBuy.RemoveAt(traderType.CountToBuy.Count - 1);
                    }
                    break;
                }
            }
            

            for (int i = 0; i < traderType.GoodsType.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(-18, true);
                EditorGUILayout.LabelField(traderType.GoodsType[i].ToString(), GUILayout.MaxWidth(100));
                EditorGUILayout.Space(3, true);
                EditorGUILayout.LabelField("coef", GUILayout.MaxWidth(30));
                traderType.Coefficient[i] = EditorGUILayout.Slider(traderType.Coefficient[i], 0.4f, 1, GUILayout.MaxWidth(60));
                EditorGUILayout.Space(1, true);
                EditorGUILayout.LabelField("countToBuy", GUILayout.MaxWidth(80));
                traderType.CountToBuy[i] = EditorGUILayout.IntField(traderType.CountToBuy[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
