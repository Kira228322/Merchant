using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeflow : MonoBehaviour, ISaveable<TimeFlowSaveData>
{
    public float TimeScale; // TimeScale должен влиять на скорость времени суток и на скорость порчи продуктов

    private float _timeCounter;

    public TimeFlowSaveData SaveData()
    {
        TimeFlowSaveData saveData = new(GameTime.CurrentDay, GameTime.Hours, GameTime.Minutes, TimeScale);
        return saveData;
    }
    public void LoadData(TimeFlowSaveData data)
    {
        GameTime.CurrentDay = data.CurrentDay;
        GameTime.Hours = data.CurrentHour;
        GameTime.Minutes = data.CurrentMinute;
        TimeScale = data.TimeScale;
    }


    private void Update()
    {
        _timeCounter += Time.deltaTime * TimeScale;
        if (_timeCounter >= 1f) 
        { 
            GameTime.Minutes++;
            _timeCounter = 0;
        }
    }
}
