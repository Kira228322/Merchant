using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PregenQuestDatabase : MonoBehaviour
{
    public PregenQuestDatabaseSO Quests;
    private static PregenQuestDatabase Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static QuestParams GetQuestParams(string summary)
    {
        PregenQuestSO pregenQuestSO = Instance.Quests.ScriptedQuests.FirstOrDefault(quest => quest.QuestSummary == summary);
        if (pregenQuestSO == null)
        {
            Debug.LogWarning(" веста с таким summary нет в базе");
            return null;
        }
        return pregenQuestSO.GenerateQuestParams();

    }
    public static QuestParams GetRandomQuest(out string name, out string description)
    {
        var pregenQuestSO = Instance.Quests.RandomQuests[Random.Range(0, Instance.Quests.RandomQuests.Count)];
        name = pregenQuestSO.Name;
        description = pregenQuestSO.Description;
        return pregenQuestSO.RandomQuest.GenerateQuestParams();
    }
}
