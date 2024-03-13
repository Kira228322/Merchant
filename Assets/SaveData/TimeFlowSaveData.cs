[System.Serializable]
public class TimeFlowSaveData
{
    public int CurrentDay;
    public int CurrentHour;
    public int CurrentMinute;
    public float TimeScale;

    public TimeFlowSaveData(int currentDay, int currentHour, int currentMinute, float timeScale)
    {
        CurrentDay = currentDay;
        CurrentHour = currentHour;
        CurrentMinute = currentMinute;
        TimeScale = timeScale;
    }
}
