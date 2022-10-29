using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Roman
{

    [CreateAssetMenu(fileName = "newItemRoman", menuName = "Item Roman")]
    public class Item : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public int Price;

        public float Weight;
        public int MaxCountInSlot;

        public int CellSizeWidth;
        public int CellSizeHeight;

        [HideInInspector] public bool IsPerishable;
        [HideInInspector] public float DaysToHalfSpoil;
        [HideInInspector] public float DaysToSpoil;

        [CustomEditor(typeof(Item))]
        public class ItemEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                Item item = (Item)target;

                EditorGUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Is Perishable?", GUILayout.MaxWidth(80));
                item.IsPerishable = EditorGUILayout.Toggle(item.IsPerishable);
                EditorGUILayout.EndHorizontal();

                if (item.IsPerishable)
                {
                    EditorGUILayout.Space(-4);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space(20, true);
                    EditorGUILayout.LabelField("Days to half spoil", GUILayout.MaxWidth(110));
                    item.DaysToHalfSpoil = EditorGUILayout.FloatField(item.DaysToHalfSpoil);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space(20, true);
                    EditorGUILayout.LabelField("Days to spoil", GUILayout.MaxWidth(110));
                    item.DaysToSpoil = EditorGUILayout.FloatField(item.DaysToSpoil);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}