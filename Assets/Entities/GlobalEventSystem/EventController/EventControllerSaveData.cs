using System;

[Serializable]
public class EventControllerSaveData
{
    public int LastEventDay { get; set; }
    public EventControllerSaveData(int lastEventDay)
    {
        LastEventDay = lastEventDay;
    }

}
