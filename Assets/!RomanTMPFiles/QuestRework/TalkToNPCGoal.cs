using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roman.Rework
{
    public class TalkToNPCGoal : Goal
    {
        public NPCData RequiredNPC;
        public string RequiredLine; //��, � ��� ���� ����������. � Ink ���������� ��� invoke(RequiredLine) ������ � Ink: invoke(talked_about_trading)
        public string FailingLine; //���� ��������� �� ����, �� ����� ����������.
        public TalkToNPCGoal(GoalParams goalParams, int requiredIDofNPC, string requiredLine, string failingLine): base(goalParams)
        {
            RequiredNPC = NPCDatabase.GetNPCData(requiredIDofNPC);
            RequiredLine = requiredLine;
            FailingLine = failingLine;
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
            if (npc.NpcData == RequiredNPC)
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
}
