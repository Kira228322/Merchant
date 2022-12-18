using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour
{
    protected string NextQuestName;
    public List<Goal> Goals { get; set; } = new();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public int MoneyReward { get; set; }
    public bool IsCompleted { get; set; }

    public void CheckGoals()
    {
        foreach (Goal goal in Goals)
        {
            if (!goal.IsCompleted)
            {
                break;
            }
        }
        Complete();

    }
    protected void Complete()
    {
        IsCompleted = true;
        GiveReward(); //Пусть QuestHandler выдает награду потом мб а не здесь? А нахуя?
        Debug.Log("Quest complete, time to feed");
        QuestHandler.RemoveQuest(this.GetType()); //Пока что решил сразу убирать этот компонент по завершении,
                                             //мб потом ещё будем сохранять его куда-то для истории в квестлоге 
        if (NextQuestName != null)
        {
            QuestHandler.AddQuest(NextQuestName);
        }

        foreach (Goal goal in Goals)
        {
            goal.Deinitialize();
        }
    }

    private void GiveReward()
    {
        Player.Singleton.AddExperience(ExperienceReward);
        Player.Singleton.Money += MoneyReward;
    }
}
