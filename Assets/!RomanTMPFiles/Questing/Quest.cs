using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Quest: MonoBehaviour
{
    protected string NextQuestName;
    public List<Goal> Goals { get; set; } = new();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public int MoneyReward { get; set; }
    public bool IsCompleted { get; set; }

    public event UnityAction QuestUpdatedEvent;
    public event UnityAction QuestCompletedEvent;

    public void CheckGoals()
    {
        foreach (Goal goal in Goals)
        {
            if (!goal.IsCompleted)
            {
                return;
            }
        }
        QuestUpdatedEvent?.Invoke();
        Complete();

    }
    protected void Complete()
    {
        IsCompleted = true;
        GiveReward(); //����� QuestHandler ������ ������� ����� �� � �� �����? � �����?
        
        if (NextQuestName != null)
        {
            QuestHandler.AddQuest(NextQuestName);
        }

        foreach (Goal goal in Goals)
        {
            goal.Deinitialize();
        }
        QuestCompletedEvent?.Invoke();
        QuestHandler.RemoveQuest(this.GetType()); //���� ��� ����� ����� ������� ���� ��������� �� ����������,
                                             //�� ����� ��� ����� ��������� ��� ����-�� ��� ������� � ��������� 
    }

    protected virtual void GiveReward()
    {
        Player.Singleton.AddExperience(ExperienceReward);
        Player.Singleton.Money += MoneyReward;
    }
    protected void QuestUpdated()
    {
        QuestUpdatedEvent?.Invoke();
    }
}
