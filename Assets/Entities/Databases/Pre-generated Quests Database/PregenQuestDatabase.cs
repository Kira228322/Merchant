using System.Linq;
using UnityEngine;

public class PregenQuestDatabase : MonoBehaviour
{
    public PregenQuestDatabaseSO Quests;
    private static PregenQuestDatabase Instance;
    public static PregenQuestDatabaseSO QuestList => Instance.Quests;
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

    public static PregenQuestSO GetPregenQuest(string summary)
    {
        PregenQuestSO pregenQuestSO = Instance.Quests.ScriptedQuests.FirstOrDefault(quest => quest.QuestSummary == summary);
        if (pregenQuestSO == null)
        {
            Debug.LogWarning(" веста с таким summary нет в базе");
            return null;
        }
        return pregenQuestSO;
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
}
