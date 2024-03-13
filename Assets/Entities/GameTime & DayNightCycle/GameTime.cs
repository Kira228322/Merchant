using UnityEngine.Events;
public static class GameTime
{

    private static Timeflow _timeflow;
    private static float _timeScaleInTravel = 30;
    public static float TimeScaleInTravel => _timeScaleInTravel;
    private static int _currentDay = 1;
    private static int _hours = 0;
    private static int _minutes = 0;

    public static event UnityAction HourChanged;
    public static event UnityAction MinuteChanged;
    public static event UnityAction DayChanged;

    public static event UnityAction<int, int, int> TimeSkipped;

    public static int CurrentDay
    {
        get => _currentDay;
        set
        {
            _currentDay = value;
            DayChanged?.Invoke();
        }
    }
    public static int Hours
    {
        get { return _hours; }
        set
        {
            _hours = value;
            if (_hours >= 24)
            {
                _hours = 0;
                CurrentDay++;
            }
            HourChanged?.Invoke();
        }
    }
    public static int Minutes
    {
        get => _minutes;
        set
        {
            _minutes = value;
            if (_minutes >= 60)
            {
                _minutes = 0;
                Hours++;
            }
            MinuteChanged?.Invoke();
        }
    }
    public static void TimeSet(int days, int hours, int minutes)
    {
        //Здесь не нужно триггерить ивенты, поэтому _hours а не Hours.
        //Это вызывается только при загрузке сохранения.
        //Для других целей стоит использовать TimeSkip(int days, int hours, int minutes)

        _currentDay = days;
        _hours = hours;
        _minutes = minutes;
    }
    public static void TimeSkip(int days, int hours, int minutes)
    {
        hours += minutes / 60;
        minutes %= 60;          // Это проверки на случай неправильных значений в аргументах 
        days += hours / 24;     // (Типа добавить 0 часов 69 минут, а не 1 час 9 минут)
        hours %= 24;



        _minutes += minutes;
        _hours += hours;
        _currentDay += days;


        if (_minutes >= 60)
        {
            _hours++;
            _minutes %= 60;
        }
        if (_hours >= 24)
        {
            _currentDay++;
            _hours %= 24;
        }

        MinuteChanged?.Invoke();
        TimeSkipped?.Invoke(days, hours, minutes);

    }

    public static void Init(Timeflow timeflow)
    {
        _timeflow = timeflow;
    }

    public static void SetTimeScale(float value)
    {
        _timeflow.TimeScale = value;
    }

    public static float GetTimeScale()
    {
        return _timeflow.TimeScale;
    }

    public static TimeFlowSaveData SaveData()
    {
        return _timeflow.SaveData();
    }
    public static void LoadData(TimeFlowSaveData data)
    {
        _timeflow.LoadData(data);
    }
}
