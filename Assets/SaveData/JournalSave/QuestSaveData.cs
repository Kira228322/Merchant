using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestSaveData
{
    public List<QuestParams> savedQuestParams = new();
    public List<AwaitingQuest> awaitingQuests = new();
}
