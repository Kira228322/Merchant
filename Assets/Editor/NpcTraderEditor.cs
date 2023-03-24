using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NpcTrader))]
public class NpcTraderDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.FindProperty("NpcData").isExpanded = false;

        DrawPropertiesExcluding(serializedObject, "NpcData");

        serializedObject.ApplyModifiedProperties();
    }
}
