using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newQuestGiverNpc", menuName = "NPCs/QuestGiverData")]
public class NpcQuestGiverData : NpcData, IResetOnExitPlaymode, ISaveable<NpcQuestGiverSaveData>
{
    [SerializeField] private List<PregenQuestSO> pregenQuests = new();

    private int _lastGiveDay = -8; //чтобы работало на 0 день

    public bool IsReadyToGiveQuest()
    {
        if (GameTime.CurrentDay > _lastGiveDay + 7)
        {
            return true;
        }
        return false;
    }
    public QuestParams GetRandomQuest()
    {
        return pregenQuests[Random.Range(0, pregenQuests.Count)].GenerateQuestParams();
    }
    //GiveRandomQuest устанавливает кулдаун, а Get - просто посмотреть.
    //Get используется в доске объявлений (кд устанавливается, когда объява берется с доски.)
    //Give используется когда квест берется ртом.
    public QuestParams GiveRandomQuest()
    {
        SetCooldown();
        List<PregenQuestSO> questsToRoll = new(pregenQuests);
        while (questsToRoll.Count != 0)
        {
            PregenQuestSO rolledQuest = questsToRoll[Random.Range(0, questsToRoll.Count)];
            if (QuestHandler.GetActiveQuestBySummary(rolledQuest.QuestSummary) != null) //такой квест уже висит
                questsToRoll.Remove(rolledQuest);
            else return rolledQuest.GenerateQuestParams();
        }
        //Если квесты, которые можно зароллить у нпс закончились в этом while => они все уже активные.
        //Если здесь возвращать null, то это нужно будет учитывать также при написании диалогов
        //Значит, об этом придется помнить и потенциально это может создать баги,
        //которые даже не выявятся во время теста и попадут в финальную игру (потому что рандом)
        //Поэтому здесь будет преграда в виде костыля что если не удалось найти уникального квеста,
        //то всё-таки выдастся дублирующийся.
        //Всё равно в игре такое вряд ли случится, потому что у квестов должен будет
        //быть срок на выполнение в 7 дней (как раз кулдаун квестгивера), но всё-таки подстрахуюсь
        return pregenQuests[Random.Range(0, pregenQuests.Count)].GenerateQuestParams();
    }
    public void SetCooldown()
    {
        _lastGiveDay = GameTime.CurrentDay;
    }
    void IResetOnExitPlaymode.ResetOnExitPlaymode()
    {
        ResetOnExitPlaymode();
        _lastGiveDay = -8;
    }

    NpcQuestGiverSaveData ISaveable<NpcQuestGiverSaveData>.SaveData()
    {
        return new(ID, CurrentMoney, _lastGiveDay);
    }

    void ISaveable<NpcQuestGiverSaveData>.LoadData(NpcQuestGiverSaveData data)
    {
        LoadData(data);

        _lastGiveDay = data.LastGiveQuestDay;
    }
}
