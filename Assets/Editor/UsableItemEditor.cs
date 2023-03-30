using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(UsableItem))]
class UsableItemEditor : Editor
{
    private UsableItem item;
    private void OnEnable()
    {
        item = (UsableItem)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        switch (item.UsableItemType)
        {
            case UsableItem.UsableType.Edible:
                EditorGUILayout.BeginHorizontal();
                
                GUILayout.Label("Food value", GUILayout.MaxWidth(80));
                item.UsableValue = EditorGUILayout.IntField(item.UsableValue);
                
                EditorGUILayout.EndHorizontal();
                break;
            case UsableItem.UsableType.Teleport:
                break;
            case UsableItem.UsableType.Bottle:
                break;
            case UsableItem.UsableType.Potion:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Effect", GUILayout.MaxWidth(60));
                item.Effect = (Status)EditorGUILayout.ObjectField(item.Effect, typeof(Status), false);
                EditorGUILayout.EndHorizontal();
                break;
            case UsableItem.UsableType.Recipe:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Recipe", GUILayout.MaxWidth(60));
                item.Recipe = (CraftingRecipe)EditorGUILayout.ObjectField(item.Recipe, typeof(CraftingRecipe), false);
                EditorGUILayout.EndHorizontal();
                break;
        }
    }
}
