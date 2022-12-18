using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public Quest Quest { get; set; } //� ������ ������ ����������� ��� ����
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public void Evaluate()
    {
        if (!IsCompleted)
        {
            if (CurrentAmount >= RequiredAmount)
            {
                Complete();
            }
        }
        else if (CurrentAmount < RequiredAmount) //������������� ����: ���� �������� ����� ������� �� ���� �����
                                                 //1) ������� 3 ������
                                                 //2) ���������� � ����� (�� ���� ������� ��� 3 ������)
                                                 //�� ���� ������ 3 ������ � ����� ���� ������� �� ������ ��� �� ��������
                                                            
        {
            IsCompleted = false; //������� CheckGoals, � �� ������������� ���������������? �������, ������ � ��� Quest, ��� ��� �����.
        }
    }
    private void Complete()
    {
        IsCompleted = true;
        Debug.Log("Goal completed");
        Quest.CheckGoals();
    }
    public virtual void Initialize()
    {

    }
}
