using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(UniqueID))]
public class UniqueIDEditor : Editor
{
    private UniqueID uniqueID;
    private void OnEnable()
    {
        uniqueID = (UniqueID)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("Только если объект на сцене (не в меню префаба):");
        if (GUILayout.Button("Сгенерировать Unique ID"))
        {
            uniqueID.GenerateNewID();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(uniqueID);
    }
    
}
