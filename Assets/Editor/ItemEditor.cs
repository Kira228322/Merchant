using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Item item = (Item)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Edible", GUILayout.MaxWidth(80));
        // item.IsEdible = EditorGUILayout.Toggle(item.IsEdible);
        EditorGUILayout.EndHorizontal();

        // if (item.IsEdible)
        // {
        //     EditorGUILayout.Space(-4);
        //     EditorGUILayout.BeginHorizontal();
        //     EditorGUILayout.Space(20, true);
        //     EditorGUILayout.LabelField("Food value", GUILayout.MaxWidth(110));
        //     item.UsableValue = EditorGUILayout.IntField(item.UsableValue);
        //     EditorGUILayout.EndHorizontal();
        // }

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