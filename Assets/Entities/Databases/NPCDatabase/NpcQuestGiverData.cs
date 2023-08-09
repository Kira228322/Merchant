using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newQuestGiverNpc", menuName = "NPCs/QuestGiverData")]
public class NpcQuestGiverData : NpcData, IResetOnExitPlaymode, ISaveable<NpcQuestGiverSaveData>
{
    [SerializeField] private List<PregenQuestSO> pregenQuests = new();

    private int _lastGiveDay = -8; //����� �������� �� 0 ����

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
    //GiveRandomQuest ������������� �������, � Get - ������ ����������.
    //Get ������������ � ����� ���������� (�� ���������������, ����� ������ ������� � �����.)
    //Give ������������ ����� ����� ������� ����.
    public QuestParams GiveRandomQuest()
    {
        SetCooldown();
        List<PregenQuestSO> questsToRoll = new(pregenQuests);
        while (questsToRoll.Count != 0)
        {
            PregenQuestSO rolledQuest = questsToRoll[Random.Range(0, questsToRoll.Count)];
            if (QuestHandler.GetActiveQuestBySummary(rolledQuest.QuestSummary) != null) //����� ����� ��� �����
                questsToRoll.Remove(rolledQuest);
            else return rolledQuest.GenerateQuestParams();
        }
        //���� ������, ������� ����� ��������� � ��� ����������� � ���� while => ��� ��� ��� ��������.
        //���� ����� ���������� null, �� ��� ����� ����� ��������� ����� ��� ��������� ��������
        //������, �� ���� �������� ������� � ������������ ��� ����� ������� ����,
        //������� ���� �� �������� �� ����� ����� � ������� � ��������� ���� (������ ��� ������)
        //������� ����� ����� �������� � ���� ������� ��� ���� �� ������� ����� ����������� ������,
        //�� ��-���� �������� �������������.
        //�� ����� � ���� ����� ���� �� ��������, ������ ��� � ������� ������ �����
        //���� ���� �� ���������� � 7 ���� (��� ��� ������� �����������), �� ��-���� ������������
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
