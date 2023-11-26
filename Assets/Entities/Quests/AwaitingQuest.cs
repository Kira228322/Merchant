using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///  вест, который ждЄт задержки, чтобы быть выданным.
/// ”ничтожает себ€ по прошествии времени, заданного в конструкторе.
/// </summary>
[Serializable]
public class AwaitingQuest

{
    public QuestParams questParams;
    public int delay;
    public event UnityAction<AwaitingQuest> AwaitingQuestGiven;
    public AwaitingQuest(QuestParams questParams, int delay)
    {
        this.questParams = questParams;
        this.delay = delay;

        GameTime.HourChanged += OnHourChanged;
    }
    private void OnHourChanged()
    {
        delay--;
        if (delay <= 0)
        {
            QuestHandler.AddQuest(questParams);
            GameTime.HourChanged -= OnHourChanged;
            AwaitingQuestGiven?.Invoke(this);
        }
    }
}
