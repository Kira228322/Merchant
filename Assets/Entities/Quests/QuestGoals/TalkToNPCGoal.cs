using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNPCGoal : Goal
{
    public NPC RequiredNPC; //Чел, с кем надо поговорить
    public TalkToNPCGoal(Quest quest, int requiredIDofNPC, string description, bool isCompleted, int currentAmount, int requiredAmount)
    {
        Quest = quest;
        RequiredNPC = NPCDatabase.GetNPC(requiredIDofNPC);
        Description = description;
        IsCompleted = isCompleted;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;
    }

    public override void Initialize()
    {
        base.Initialize();

        //subscribe to NPC's TalkEvent

        Evaluate();
    }

    public override void Deinitialize()
    {
        base.Deinitialize();

        //unsubscribe from NPC's TalkEvent
    }

    //do sth when event invoked
}
