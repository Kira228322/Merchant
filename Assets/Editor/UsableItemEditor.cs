using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(UsableItem))]
class UsableItemEditor : ItemEditor
{
    private UsableItem usableItem;
    protected override void OnEnable()
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

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Gives quest after use?");
        usableItem.GivesQuestAfterUse = EditorGUILayout.Toggle(usableItem.GivesQuestAfterUse);
        EditorGUILayout.EndHorizontal();

        if (usableItem.GivesQuestAfterUse)
        {
            GUILayout.Label("Given quest's summary:");
            usableItem.QuestSummaryGivenAfterUse = EditorGUILayout.TextArea(usableItem.QuestSummaryGivenAfterUse, GUILayout.Height(20));
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Gives items after use?*", "These items should be 1x1"));
        usableItem.GivesItemsAfterUse = EditorGUILayout.Toggle(usableItem.GivesItemsAfterUse);
        EditorGUILayout.EndHorizontal();

        if (usableItem.GivesItemsAfterUse)
        {
            foreach (var item in usableItem.ItemsGivenAfterUse)
            {
                GUILayout.Space(-1);
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(-20);
                if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                {
                    usableItem.ItemsGivenAfterUse.Remove(item);
                    break;
                }
                GUILayout.Label("Name", GUILayout.MaxWidth(38));
                item.itemName = EditorGUILayout.TextField(item.itemName);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Amount", GUILayout.MaxWidth(50));
                item.amount = EditorGUILayout.IntField(item.amount);
                GUILayout.Label("DayBoughtAgo", GUILayout.MaxWidth(80));
                item.daysBoughtAgo = EditorGUILayout.FloatField(item.daysBoughtAgo);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            if (GUILayout.Button("Add item"))
                usableItem.ItemsGivenAfterUse.Add(new("", 0, 0));
        }

        if (GUI.changed)
            EditorUtility.SetDirty(usableItem);
    }
}
