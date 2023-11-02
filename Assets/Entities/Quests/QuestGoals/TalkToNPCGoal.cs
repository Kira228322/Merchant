using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class TalkToNPCGoal : Goal
{
    [NonSerialized]public NpcData RequiredNPC;
    public int RequiredIDOfNPC;
    public string RequiredLine; //То, о чём надо поговорить. В Ink вызывается как invoke(RequiredLine) пример в Ink: invoke(talked_about_trading)
    public string FailingLine; //Если поговорил об этом, то квест зафейлится.
    public TalkToNPCGoal(State currentState, string description, int currentAmount, int requiredAmount, int requiredIDofNPC, string requiredLine, string failingLine) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredIDOfNPC = requiredIDofNPC;
        RequiredLine = requiredLine;
        FailingLine = failingLine;
    }

    public override void Initialize()
    {
        RequiredNPC = NpcDatabase.GetNPCData(RequiredIDOfNPC);

        DialogueManager.Instance.TalkedToNPCAboutSomething += OnTalkWithNPC;

        Evaluate();
    }

    public override void Deinitialize()
    {
        DialogueManager.Instance.TalkedToNPCAboutSomething -= OnTalkWithNPC;
    }

    private void OnTalkWithNPC(NpcData npcData, string line)
    {
        if (npcData == RequiredNPC)
        {
            if (line == RequiredLine)
            {
                CurrentAmount++;
                Evaluate();
            }
            if (line == FailingLine)
            {
                Fail();
            }
        }
    }
}