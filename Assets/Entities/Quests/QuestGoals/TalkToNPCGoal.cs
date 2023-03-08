using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNPCGoal : Goal
{
    public NPCData RequiredNPC; //���, � ��� ���� ����������
    public string RequiredLine; //��, � ��� ���� ���������� (� Ink ���������� ��� #invoke something, ��� ��� something � ���� RequiredLine)
    public TalkToNPCGoal(Quest quest, int requiredIDofNPC, string requiredLine, string description, bool isCompleted, int currentAmount, int requiredAmount)
    {
        Quest = quest;
        RequiredNPC = NPCDatabase.GetNPCData(requiredIDofNPC);
        RequiredLine = requiredLine;
        Description = description;
        IsCompleted = isCompleted;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;
    }

    public override void Initialize()
    {
        base.Initialize();

        DialogueManager.Instance.TalkedToNPCAboutSomething += OnTalkWithNPC;

        Evaluate();
    }

    public override void Deinitialize()
    {
        base.Deinitialize();

        DialogueManager.Instance.TalkedToNPCAboutSomething -= OnTalkWithNPC;
    }

    private void OnTalkWithNPC(NPC npc, string line)
    {
        if (npc.NpcData == RequiredNPC && line == RequiredLine)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
