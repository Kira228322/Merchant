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
        PregenQuestSO pregenQuestSO = Instance.Quests.PregenQuests.FirstOrDefault(quest => quest.QuestSummary == summary);
        if (pregenQuestSO == null)
        {
            Debug.LogWarning(" веста с таким summary нет в базе");
            return null;
        }
        QuestParams result = pregenQuestSO.GenerateQuestParams();
        return result;

    }
}
