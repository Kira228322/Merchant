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
            _region._coefsForItemTypes = new ();
            _region._coefsForItemTypes.Clear();
            _region.tmp = new List<float>();
            
            foreach (var itemType in Enum.GetValues(typeof(Item.ItemType)))
            {
                _region._coefsForItemTypes.Add((Item.ItemType)itemType, 1);
                _region.tmp.Add(1);
            }
            
        }
        
        GUILayout.Space(5);
        int i = 0;
        if (_region.tmp.Count > 0 && _region._coefsForItemTypes.Count > 0)
            foreach (var objItemType in Enum.GetValues(typeof(Item.ItemType)))
            {
                Item.ItemType itemType = (Item.ItemType)objItemType;
                EditorGUILayout.BeginVertical();
            
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(itemType.ToString());
                GUILayout.FlexibleSpace();
                _region.tmp[i] = EditorGUILayout.FloatField(_region.tmp[i]);
                i++;
                EditorGUILayout.EndHorizontal();
            
                EditorGUILayout.EndVertical();
            }

        i = 0;
        foreach (var objItemType in Enum.GetValues(typeof(Item.ItemType)))
        {
            Item.ItemType itemType = (Item.ItemType)objItemType;
            _region._coefsForItemTypes[itemType] = _region.tmp[i];
            i++;
        }
        
        if (GUI.changed)
            EditorUtility.SetDirty(_region);
    }
    
    
}
