using Codice.CM.SEIDInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(UsableItem))]
class UsableItemEditor : ItemEditor
{
    private UsableItem usableItem;
    protected override  void OnEnable()
    {
        usableItem = (UsableItem)target;
        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        switch (usableItem.UsableItemType)
        {
            case UsableItem.UsableType.Edible:
                EditorGUILayout.BeginHorizontal();
                
                GUILayout.Label("Food value", GUILayout.MaxWidth(80));
                usableItem.UsableValue = EditorGUILayout.IntField(usableItem.UsableValue);
                
                EditorGUILayout.EndHorizontal();
                break;
            case UsableItem.UsableType.Energetic:
                EditorGUILayout.BeginHorizontal();
                
                GUILayout.Label("Food value", GUILayout.MaxWidth(120));
                usableItem.UsableValue = EditorGUILayout.IntField(usableItem.UsableValue);
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                
                GUILayout.Label("SleepRestore value", GUILayout.MaxWidth(120));
                usableItem.SecondValue = EditorGUILayout.IntField(usableItem.SecondValue);
                
                EditorGUILayout.EndHorizontal();
                break;
            case UsableItem.UsableType.Teleport:
                break;
            case UsableItem.UsableType.Bottle:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Food value", GUILayout.MaxWidth(80));
                usableItem.UsableValue = EditorGUILayout.IntField(usableItem.UsableValue);
                EditorGUILayout.EndHorizontal();
                break;
            case UsableItem.UsableType.Potion:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Effect", GUILayout.MaxWidth(60));
                usableItem.Effect = (Status)EditorGUILayout.ObjectField(usableItem.Effect, typeof(Status), false);
                EditorGUILayout.EndHorizontal();
                break;
            case UsableItem.UsableType.Recipe:
                
                if (GUILayout.Button("Добавить рецепт крафта в список"))
                    usableItem.Recipes.Add(CreateInstance<CraftingRecipe>());
                
                
                if (usableItem.Recipes.Count > 0)
                {
                    for (int i = 0; i < usableItem.Recipes.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                        {
                            usableItem.Recipes.RemoveAt(i);
                            break;
                        }
                        GUILayout.Label("Recipe", GUILayout.MaxWidth(60));
                        usableItem.Recipes[i] = (CraftingRecipe)EditorGUILayout.ObjectField(usableItem.Recipes[i], typeof(CraftingRecipe), false);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                
                
                break;
            case UsableItem.UsableType.Note:
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Note header", GUILayout.MaxWidth(80));
                EditorStyles.textField.wordWrap = false;
                usableItem.NoteHeader = EditorGUILayout.TextArea(usableItem.NoteHeader, GUILayout.Height(20));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Note text", GUILayout.MaxWidth(80));
                EditorStyles.textField.wordWrap = true;
                usableItem.NoteText = EditorGUILayout.TextArea(usableItem.NoteText, GUILayout.Height(256));
                EditorGUILayout.EndHorizontal();
                break;
        }
        if (GUI.changed)
            EditorUtility.SetDirty(usableItem);
    }
}
