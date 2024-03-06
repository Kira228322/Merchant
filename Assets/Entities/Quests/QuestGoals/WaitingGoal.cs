/// <summary>
/// Цель, которая выполняется по прошествии RequiredAmount часов
///Не путать с TimedGoal <см. cref ="TimedGoal" />
/// </summary>
[System.Serializable]
public class WaitingGoal : Goal
{
    private int _timeCounter;
    public WaitingGoal(State currentState, string description, int currentAmount, int requiredAmount) : base(currentState, description, currentAmount, requiredAmount)
    {
        //аналогичен базовому конструктору
    }

    public override void Initialize()
    {
        Evaluate();

        GameTime.MinuteChanged += OnMinuteChanged;
        GameTime.TimeSkipped += OnTimeSkipped;
    }
    public override void Deinitialize()
    {
        GameTime.MinuteChanged -= OnMinuteChanged;
        GameTime.TimeSkipped -= OnTimeSkipped;
    }

    private void OnMinuteChanged()
    {
        _timeCounter++;
        if (_timeCounter >= 60)
        {
            _timeCounter = 0;
            CurrentAmount++;
            Evaluate();
        }
    }

    private void OnTimeSkipped(int days, int hours, int minutes)
    {
        int totalMinutesSkipped = (days * 24 + hours) * 60 + minutes;
        _timeCounter += totalMinutesSkipped;
        if (_timeCounter >= 60)
        {
            _timeCounter = totalMinutesSkipped % 60;
            CurrentAmount += totalMinutesSkipped / 60;
            Evaluate();
        }
    }

    protected override void Evaluate()
    {

        if (CurrentState == State.Active)
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Completed;
                Deinitialize(); //Можно перестать считать, ведь время не уйдет назад
            }
        }

        UpdateGoal();
    }
}