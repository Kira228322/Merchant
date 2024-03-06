using System.Collections.Generic;

[System.Serializable]
public class QuestSaveData
{
    public List<QuestParams> savedQuestParams = new();
    public List<AwaitingQuest> awaitingQuests = new();
}
