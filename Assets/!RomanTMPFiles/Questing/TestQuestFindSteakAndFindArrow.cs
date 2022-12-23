using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestFindSteakAndFindArrow : Quest
{

    void Awake()
    {
        QuestName = "SteakThenArrowFinding";
        Description = "Find 1 steak and find 1 arrow";
        ExperienceReward = 100;
        MoneyReward = 69;

        Goals.Add(new CollectItemsGoal(this, "Steak", "������ 1 steak", false, 0, 1));
        Goals.Add(new CollectItemsGoal(this, "Arrow", "������ 1 ������", false, 0, 1));

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
            goal.GoalUpdated += QuestUpdated;
        }
    }
    /*public override void CheckGoals()
    {
        
        /* ��������� �� ����� ������� �������������, �� ���������� � ������.
         * ������� ���� � ���, ��� ������ ������ ������ ����� ������� ������� X, ����� ������� Y
         * ���� �� ������ X, ������ ������� Y
         * ����� ������ ����� 2 ��������� ������: �������� X, ��������� ����� �������� Y
         
          if (!Goals[1].IsCompleted)
        {
            Goals[1].Initialize();
        }
        else Complete();
        */
        /* ��� ���� ����� �� ������: ���� ������� CheckGoals, ������ ����� �� Goal ����� ����������. ���� ��� ��� �� ������, ������ ��� ������ 
         ����, �� ���� ����� ������� ���� ����� �� ��� ����� � �������� �������������, �������� ��� ��� ��� �����:
         if (!Goals[2].IsCompleted && Goals[1].IsCompleted) 
        {
            Goals[2].Initialize();
        }
        else if (!Goals[1].IsCompleted)
        {
            Goals[1].Initialize();
        }
        else Complete();
        
      } */
}
