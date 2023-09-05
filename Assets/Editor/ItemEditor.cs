using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    private Item item;
    protected virtual  void OnEnable()
    {
        item = (Item)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        item.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", item.Icon, typeof(Sprite), false);
        
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Quest Item", GUILayout.MaxWidth(80));
        item.IsQuestItem = EditorGUILayout.Toggle(item.IsQuestItem);
        EditorGUILayout.EndHorizontal();

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