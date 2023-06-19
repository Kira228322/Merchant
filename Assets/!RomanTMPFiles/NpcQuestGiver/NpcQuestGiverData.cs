using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newQuestGiverNpc", menuName = "NPCs/QuestGiverData")]
public class NpcQuestGiverData : NpcData, IResetOnExitPlaymode //TODO ISaveable<>
{
    [SerializeField] private List<PregenQuestSO> pregenQuests = new();

    private int _lastGiveDay = -8; //чтобы работало на 0 день

    public bool IsReadyToGiveQuest()
    {
        _lastGiveDay = -8;
        if (GameTime.CurrentDay > _lastGiveDay + 7)
        {
            return true;
        }
        return false;
    }

    public QuestParams GiveRandomQuest()
    {
        _lastGiveDay = GameTime.CurrentDay;
        return pregenQuests[Random.Range(0, pregenQuests.Count)].GenerateQuestParams();
    }

}
