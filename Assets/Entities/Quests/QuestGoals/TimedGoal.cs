/// <summary>
/// ����, ������� ������������� �� ���������� RequiredAmount �����
///�� ������ � WaitingGoal <��. cref ="WaitingGoal" />
/// </summary>
[System.Serializable]
public class TimedGoal : Goal
{
    private int _timeCounter;
    public TimedGoal(State currentState, string description, int currentAmount, int requiredAmount) : base(currentState, description, currentAmount, requiredAmount)
    {
        //���������� �������� ������������
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

    protected override void Evaluate()
    {
        if (CurrentState == State.Completed) //TimedGoal ���������� ��������, �� ����� ��������� ������� �� ������
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Failed;
                Deinitialize(); //����� ��������� �������, ���� ����� �� ����� �����
            }

        }
        UpdateGoal();
    }
}
