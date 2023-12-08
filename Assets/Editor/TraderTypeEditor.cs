using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TraderType))]
class EditorTraderType : Editor
{
    private TraderType traderType;
    private void OnEnable()
    {
        traderType = (TraderType)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Добавить тип предмета"))
            traderType.TraderGoodTypes.Add(new TraderType.TraderGoodType());

        if (traderType.TraderGoodTypes.Count > 0)
        {
            foreach (var goodType in traderType.TraderGoodTypes)
            {
                GUILayout.Space(-1);
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                {
                    traderType.TraderGoodTypes.Remove(goodType);
                    break;
                }
                GUILayout.FlexibleSpace();
                GUILayout.Label("Item type", GUILayout.MaxWidth(60));
                goodType.ItemType = (Item.ItemType)EditorGUILayout.EnumPopup(goodType.ItemType);
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Coefficient", GUILayout.MaxWidth(80));
                goodType.Coefficient = EditorGUILayout.Slider(goodType.Coefficient, 0.7f, 1);
                GUILayout.FlexibleSpace();
                GUILayout.Label("Count to buy", GUILayout.MaxWidth(80));
                goodType.CountToBuy = EditorGUILayout.IntField(goodType.CountToBuy);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
        }

        if (GUI.changed)
            EditorUtility.SetDirty(traderType);
    }
}