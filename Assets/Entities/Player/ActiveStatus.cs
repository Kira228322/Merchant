using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine;

public class ActiveStatus
{
    public bool IsActive { get; private set; }
    public Status StatusData { get; private set; }
    public float CurrentDurationHours { get; private set; }
    public event UnityAction StatusUpdated;
    
    public void Init(Status status)
    {
        StatusData = status;
        if (status.Type == Status.StatusType.Buff)
            CurrentDurationHours = status.HourDuration * Player.Instance.Statistics.StatusDurationModifier;
        else
            CurrentDurationHours = status.HourDuration;
        Activate();
    }
    public void RefreshStatus(Status newStatus)
    {
        if (StatusData.Type == Status.StatusType.Buff)
        {
            if (CurrentDurationHours < newStatus.HourDuration * Player.Instance.Statistics.StatusDurationModifier)
                CurrentDurationHours = StatusData.HourDuration * Player.Instance.Statistics.StatusDurationModifier;
        }
        else
        {
            if (CurrentDurationHours < newStatus.HourDuration)
                CurrentDurationHours = StatusData.HourDuration;
        }
    }
    public void SetDuration(float duration)
    {
        CurrentDurationHours = duration;
    }
    public void DecreaseDuration()
    {
        CurrentDurationHours -= 0.01667f; // 1/60;
        if (CurrentDurationHours <= 0)
        {
            Deactivate();
        }
        StatusUpdated?.Invoke();
    }
    
    public void DecreaseDuration(float hoursSkipped)
    {
        CurrentDurationHours -= hoursSkipped;
        if (CurrentDurationHours <= 0)
        {
            Deactivate();
        }
        StatusUpdated?.Invoke();
    }


    public void Activate()
    {
        foreach (var effect in StatusData.Effects)
        {
            switch (effect.stat)
            {
                case Status.Effect.Stat.Luck:
                    Player.Instance.Statistics.Luck.Additional += effect.value;
                    break;
                case Status.Effect.Stat.ExpGain:
                    Player.Instance.Experience.ExpGain += effect.value / 100f;
                    break;
                case Status.Effect.Stat.Diplomacy:
                    Player.Instance.Statistics.Diplomacy.Additional += effect.value;
                    break;
                case Status.Effect.Stat.Toughness:
                    Player.Instance.Statistics.Toughness.Additional += effect.value;
                    break;
                case Status.Effect.Stat.MoveSpeed:
                    Player.Instance.PlayerMover.SpeedModifier += effect.value / 100f;
                    Player.Instance.PlayerMover.ChangeCurrentSpeed();
                    break;
                case Status.Effect.Stat.StatusDurationModifier:
                    Player.Instance.Statistics.StatusDurationModifier += effect.value / 100f;
                    break;
                    // TODO добавлять новые действия с появлением статов
            }
        }
        IsActive = true;
    }
    public void Deactivate()
    {
        foreach (var effect in StatusData.Effects)
        {
            switch (effect.stat)
            {
                case Status.Effect.Stat.Luck:
                    Player.Instance.Statistics.Luck.Additional -= effect.value;
                    break;
                case Status.Effect.Stat.ExpGain:
                    Player.Instance.Experience.ExpGain -= effect.value / 100f;
                    break;
                case Status.Effect.Stat.Diplomacy:
                    Player.Instance.Statistics.Diplomacy.Additional -= effect.value;
                    break;
                case Status.Effect.Stat.Toughness:
                    Player.Instance.Statistics.Toughness.Additional -= effect.value;
                    break;
                case Status.Effect.Stat.MoveSpeed:
                    Player.Instance.PlayerMover.SpeedModifier -= effect.value / 100f;
                    Player.Instance.PlayerMover.ChangeCurrentSpeed();
                    break;
                case Status.Effect.Stat.StatusDurationModifier:
                    Player.Instance.Statistics.StatusDurationModifier -= effect.value / 100f;
                    break;
                    // TODO добавлять новые действия с появлением статов
            }
        }
        
        IsActive = false;
    }
}
