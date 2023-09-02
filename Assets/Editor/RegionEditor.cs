using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Region))]
public class RegionEditor : Editor
{
    private Region _region;
    private void OnEnable()
    {
        _region = (Region)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUILayout.LabelField("Coefs for item types");
        if (GUILayout.Button("Reset"))
        {
            _region.tmpFloat = new List<float>();
        
            foreach (var _ in Enum.GetValues(typeof(Item.ItemType)))
            {
                _region.tmpFloat.Add(1);
            }
        }

        if (GUI.changed)
            EditorUtility.SetDirty(_region);
        
        if (_region.tmpFloat.Count != Enum.GetValues(typeof(Item.ItemType)).Length)
        {
            return;
        }
    
        GUILayout.Space(5);
        int i = 0;
        if (_region.tmpFloat.Count > 0)
        {
            foreach (var objItemType in Enum.GetValues(typeof(Item.ItemType)))
            {
                Item.ItemType itemType = (Item.ItemType)objItemType;
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(itemType.ToString());
                GUILayout.FlexibleSpace();
                _region.tmpFloat[i] = EditorGUILayout.FloatField(_region.tmpFloat[i]);
                i++;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
        }
        
        if (GUI.changed)
            EditorUtility.SetDirty(_region);
        
    }
    
    
}
