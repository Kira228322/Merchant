using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "newItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public string Name;
    [TextArea(2,5)]public string Description;
    public Sprite Icon;
    public int Price;
    [Range(0,100)]public int Flagility; // Величина, показывающая насколько хрупкий предмет.Чем больше - тем более хрупок. max = 100
        //  Влият на вероятность разбиться во время перевозки. Например ваза будет иметь 50, а у яблок = 7.
        // Так же за то, разобьется предмет или нет отвечает "качество" дороги - ее параметр.

    public float Weight;
    public int MaxItemsInAStack;

    public int CellSizeWidth;
    public int CellSizeHeight;

    [HideInInspector] public bool IsPerishable;
    [HideInInspector] public float _daysToHalfSpoil;
    [HideInInspector] public float _daysToSpoil;
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Item item = (Item)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Perishable", GUILayout.MaxWidth(80));
            item.IsPerishable = EditorGUILayout.Toggle(item.IsPerishable);
            EditorGUILayout.EndHorizontal();

            if (item.IsPerishable)
            {
                EditorGUILayout.Space(-4);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(20, true);
                EditorGUILayout.LabelField("Days to half spoil", GUILayout.MaxWidth(110));
                item._daysToHalfSpoil = EditorGUILayout.FloatField(item._daysToHalfSpoil);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(20, true);
                EditorGUILayout.LabelField("Days to spoil", GUILayout.MaxWidth(110));
                item._daysToSpoil = EditorGUILayout.FloatField(item._daysToSpoil);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}