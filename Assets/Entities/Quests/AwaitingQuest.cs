using System;
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

    }
    public void Initialize()
    {
        GameTime.HourChanged += OnHourChanged;
        GameTime.TimeSkipped += OnTimeSkipped;
    }
    private void OnHourChanged()
    {
        delay--;
        if (delay <= 0)
            Trigger();
    }
    private void OnTimeSkipped(int days, int hours, int minutes)
    {
        int totalHoursSkipped = days * 24 + hours;
        delay -= totalHoursSkipped;
        if (delay <= 0)
            Trigger();
    }

    private void Trigger()
    {
        QuestHandler.AddQuest(questParams);
        GameTime.HourChanged -= OnHourChanged;
        GameTime.TimeSkipped -= OnTimeSkipped;
        AwaitingQuestGiven?.Invoke(this);
    }
}
