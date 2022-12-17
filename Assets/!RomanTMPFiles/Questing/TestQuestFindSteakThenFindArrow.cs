using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestFindSteakThenFindArrow : Quest
{
    //����� � ���, ��� ��� ���� �������: ������� ����� ����� steak, � ������ ����� ����� (���� ���� steak ���� � ���������) ����� ����� ������

    void Start()
    {
        QuestName = "SteakThenArrowFinding";
        Description = "Find 1 steak, after that find 1 arrow";
        ExperienceReward = 100;
        MoneyReward = 69;

        Goals.Add(new CollectItemsGoal(this, "Steak", "������ 1 steak", false, 0, 1));
        Goals.Add(new CollectItemsGoal(this, "Arrow", "������ 1 ������", false, 0, 1));

        Goals[0].Initialize();
        //�� ����, ����� ���������������� Goal2 ����� ����, ��� ���������� Goal1

    }
    public override void CheckGoals()
    {
        if (!Goals[1].IsCompleted)
        {
            Goals[1].Initialize();
        }
        else Complete();

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
        */
    }
}
