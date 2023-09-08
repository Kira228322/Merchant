using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(UsableItem))]
class UsableItemEditor : ItemEditor
{
    private UsableItem item;
    protected override  void OnEnable()
    {
        item = (UsableItem)target;
        base.OnEnable();
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
            case UsableItem.UsableType.Energetic:
                EditorGUILayout.BeginHorizontal();
                
                GUILayout.Label("Food value", GUILayout.MaxWidth(120));
                item.UsableValue = EditorGUILayout.IntField(item.UsableValue);
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                
                GUILayout.Label("SleepRestore value", GUILayout.MaxWidth(120));
                item.SecondValue = EditorGUILayout.IntField(item.SecondValue);
                
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
                
                if (GUILayout.Button("Добавить рецепт крафта в список"))
                    item.Recipes.Add(CreateInstance<CraftingRecipe>());
                
                
                if (item.Recipes.Count > 0)
                {
                    for (int i = 0; i < item.Recipes.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                        {
                            item.Recipes.RemoveAt(i);
                            break;
                        }
                        GUILayout.Label("Recipe", GUILayout.MaxWidth(60));
                        item.Recipes[i] = (CraftingRecipe)EditorGUILayout.ObjectField(item.Recipes[i], typeof(CraftingRecipe), false);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                
                
                break;
            case UsableItem.UsableType.Note:
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Text to add", GUILayout.MaxWidth(80));
                EditorStyles.textField.wordWrap = true;
                item.NoteText = EditorGUILayout.TextArea(item.NoteText, GUILayout.Height(80));
                EditorGUILayout.EndHorizontal();
                break;
        }
    }
}
