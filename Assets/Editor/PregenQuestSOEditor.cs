using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PregenQuestSO))]
class EditorPregenQuest : Editor
{
    private PregenQuestSO _quest;
    private Item.ItemType tmpItemCategory;
    private void OnEnable()
    {
        _quest = (PregenQuestSO)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Add goal"))
            _quest.goals.Add(new PregenQuestSO.CompactedGoal());


        if (_quest.goals.Count > 0)
        {
            foreach (var goal in _quest.goals)
            {
                GUILayout.Space(-1);
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                {
                    _quest.goals.Remove(goal);
                    break;
                }
                GUILayout.Space(10);
                goal.goalType = (PregenQuestSO.CompactedGoal.GoalType)EditorGUILayout.EnumPopup("Goal type", goal.goalType);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();
                goal.goalState = (Goal.State)EditorGUILayout.EnumPopup("Goal state", goal.goalState);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUIStyle style = new(EditorStyles.textField)
                {
                    wordWrap = true
                };
                goal.description = EditorGUILayout.TextField("Description", goal.description, style,
                    GUILayout.ExpandHeight(true), GUILayout.MaxHeight(25));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                goal.currentAmount = EditorGUILayout.IntField("currentAmount", goal.currentAmount);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Required Amount: ");
                GUILayout.Label("Random", GUILayout.MaxWidth(50));
                goal.randomAmount = EditorGUILayout.Toggle(goal.randomAmount, GUILayout.MaxWidth(20));
                if (goal.randomAmount)
                {
                    GUILayout.Label("min", GUILayout.MaxWidth(25));
                    goal.minRequiredAmount =
                        EditorGUILayout.IntField(goal.minRequiredAmount);
                    GUILayout.Label("max", GUILayout.MaxWidth(29));
                    goal.maxRequiredAmount =
                        EditorGUILayout.IntField(goal.maxRequiredAmount);
                }
                else
                {
                    GUILayout.Label("Amount", GUILayout.MaxWidth(50));
                    goal.minRequiredAmount = EditorGUILayout.IntField(goal.minRequiredAmount);
                    goal.maxRequiredAmount = goal.minRequiredAmount;
                }
                EditorGUILayout.EndHorizontal();

                switch (goal.goalType)
                {
                    case PregenQuestSO.CompactedGoal.GoalType.CollectItemsGoal:
                        GUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredItemName = EditorGUILayout.TextField("Required item name", goal.RequiredItemName);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        goal.AdditiveMoneyReward = GUILayout.Toggle(goal.AdditiveMoneyReward, "Additive money reward");
                        EditorGUILayout.EndHorizontal();
                        break;
                    case PregenQuestSO.CompactedGoal.GoalType.TalkToNPCGoal:
                        GUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredIDofNPC = EditorGUILayout.IntField("Required ID of NPC", goal.RequiredIDofNPC);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredLine = EditorGUILayout.TextField("Required line", goal.RequiredLine);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        goal.FailingLine = EditorGUILayout.TextField("Failing line", goal.FailingLine);
                        EditorGUILayout.EndHorizontal();
                        break;
                    case PregenQuestSO.CompactedGoal.GoalType.GiveItemsGoal:
                        GUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredIDofNPC = EditorGUILayout.IntField("Required ID of NPC", goal.RequiredIDofNPC);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredItemName = EditorGUILayout.TextField("Required item name", goal.RequiredItemName);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredLine = EditorGUILayout.TextField("Required line", goal.RequiredLine);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        goal.AdditiveMoneyReward = GUILayout.Toggle(goal.AdditiveMoneyReward, "Additive money reward");
                        EditorGUILayout.EndHorizontal();
                        break;
                    case PregenQuestSO.CompactedGoal.GoalType.DeliveryGoal:
                        GUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredIDofNPC = EditorGUILayout.IntField(new GUIContent("Required ID of NPC*", "�� ������ 07.10.23 ������������ ������ � ���������� ���������������� �����"), goal.RequiredIDofNPC);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        goal.QuestItemsBehaviour = (ItemContainer.QuestItemsBehaviourEnum)EditorGUILayout.EnumPopup("Quest items behaviour", goal.QuestItemsBehaviour);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Required Weight:");
                        GUILayout.Label("Random", GUILayout.MaxWidth(50));
                        goal.RandomDeliveryWeight = EditorGUILayout.Toggle(goal.RandomDeliveryWeight, GUILayout.MaxWidth(20));
                        if (goal.RandomDeliveryWeight)
                        {
                            GUILayout.Label("min", GUILayout.MaxWidth(25));
                            goal.MinRequiredDeliveryWeight =
                                EditorGUILayout.FloatField(goal.MinRequiredDeliveryWeight);
                            GUILayout.Label("max", GUILayout.MaxWidth(29));
                            goal.MaxRequiredDeliveryWeight =
                                EditorGUILayout.FloatField(goal.MaxRequiredDeliveryWeight);
                        }
                        else
                        {
                            GUILayout.Label("Weight", GUILayout.MaxWidth(50));
                            goal.MinRequiredDeliveryWeight = EditorGUILayout.FloatField(goal.MinRequiredDeliveryWeight);
                            goal.MaxRequiredDeliveryWeight = goal.MinRequiredDeliveryWeight;
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Required Count:");
                        GUILayout.Label("Random", GUILayout.MaxWidth(50));
                        goal.RandomDeliveryCount = EditorGUILayout.Toggle(goal.RandomDeliveryCount, GUILayout.MaxWidth(20));
                        if (goal.RandomDeliveryCount)
                        {
                            GUILayout.Label("min", GUILayout.MaxWidth(25));
                            goal.MinRequiredDeliveryCount =
                                EditorGUILayout.IntField(goal.MinRequiredDeliveryCount);
                            GUILayout.Label("max", GUILayout.MaxWidth(29));
                            goal.MaxRequiredDeliveryCount =
                                EditorGUILayout.IntField(goal.MaxRequiredDeliveryCount);
                        }
                        else
                        {
                            GUILayout.Label("Count", GUILayout.MaxWidth(50));
                            goal.MinRequiredDeliveryCount = EditorGUILayout.IntField(goal.MinRequiredDeliveryCount);
                            goal.MaxRequiredDeliveryCount = goal.MinRequiredDeliveryCount;
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredRotThreshold = EditorGUILayout.Slider(new GUIContent("Accepted rot value*", "value > 0: only items fresher than value*100% \nvalue == 0: ignored \nvalue < 0: only items more expired than |value|*100% "), goal.RequiredRotThreshold, -1, 1);
                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Accepted item categories:");
                        EditorGUILayout.EndHorizontal();

                        if (goal.RequiredItemCategories != null)
                        {
                            if (goal.RequiredItemCategories.Count == 0)
                                EditorGUILayout.LabelField("Everything is accepted");
                            else
                            {
                                foreach (var type in goal.RequiredItemCategories)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                                    {
                                        goal.RequiredItemCategories.Remove(type);
                                        break;
                                    }
                                    EditorGUILayout.LabelField(type.ToString());
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                        }
                        else
                        {
                            goal.RequiredItemCategories = new();
                        }

                        tmpItemCategory = (Item.ItemType)EditorGUILayout.EnumPopup("Select category", tmpItemCategory);

                        if (GUILayout.Button("Add accepted Category"))
                            goal.RequiredItemCategories.Add(tmpItemCategory);

                        break;
                    case PregenQuestSO.CompactedGoal.GoalType.UseItemsGoal:
                        GUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredItemName = EditorGUILayout.TextField("Required item name", goal.RequiredItemName);
                        EditorGUILayout.EndHorizontal();
                        break;
                    case PregenQuestSO.CompactedGoal.GoalType.KeepItemsGoal:
                        GUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredItemName = EditorGUILayout.TextField("Required item name", goal.RequiredItemName);
                        EditorGUILayout.EndHorizontal();
                        break;
                    case PregenQuestSO.CompactedGoal.GoalType.StayOnSceneGoal:
                        GUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        goal.RequiredSceneName = EditorGUILayout.TextField("Required scene name", goal.RequiredSceneName);
                        EditorGUILayout.EndHorizontal();
                        break;

                }

                EditorGUILayout.EndVertical();
            }
        }

        GUILayout.Space(5);
        EditorGUILayout.LabelField("Reward", EditorStyles.boldLabel);
        GUILayout.Space(5);
        _quest.RandomExp = GUILayout.Toggle(_quest.RandomExp, "Random experience");

        if (_quest.RandomExp)
        {
            EditorGUILayout.BeginHorizontal();
            _quest.MinExperienceReward = EditorGUILayout.IntField("Min exp", _quest.MinExperienceReward);
            GUILayout.FlexibleSpace();
            _quest.MaxExperienceReward = EditorGUILayout.IntField("Max exp", _quest.MaxExperienceReward);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            _quest.MinExperienceReward = EditorGUILayout.IntField("Experience", _quest.MinExperienceReward);
            _quest.MaxExperienceReward = _quest.MinExperienceReward;
        }

        _quest.RandomReward = GUILayout.Toggle(_quest.RandomReward, "Random money");

        if (_quest.RandomReward)
        {
            EditorGUILayout.BeginHorizontal();
            _quest.MinMoneyReward = EditorGUILayout.IntField("Min money", _quest.MinMoneyReward);
            GUILayout.FlexibleSpace();
            _quest.MaxMoneyReward = EditorGUILayout.IntField("Max money", _quest.MaxMoneyReward);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            _quest.MinMoneyReward = EditorGUILayout.IntField("Money", _quest.MinMoneyReward);
            _quest.MaxMoneyReward = _quest.MinMoneyReward;
        }



        GUILayout.Space(10);

        if (GUILayout.Button("Add item reward"))
            _quest.ItemRewards.Add(new ItemReward("", 0, 0));

        if (_quest.ItemRewards.Count > 0)
        {
            foreach (var item in _quest.ItemRewards)
            {
                GUILayout.Space(-1);
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(-20);
                if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                {
                    _quest.ItemRewards.Remove(item);
                    break;
                }
                GUILayout.FlexibleSpace();
                GUILayout.Label("Name", GUILayout.MaxWidth(38));
                item.itemName = EditorGUILayout.TextField(item.itemName);
                GUILayout.Label("Amount", GUILayout.MaxWidth(50));
                item.amount = EditorGUILayout.IntField(item.amount);
                GUILayout.Label("DayBoughtAg", GUILayout.MaxWidth(80));
                item.daysBoughtAgo = EditorGUILayout.FloatField(item.daysBoughtAgo);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_quest);
        }

    }
}