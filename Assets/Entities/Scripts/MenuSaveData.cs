using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MenuSaveData
{
    public float SoundSliderValue;
    public float MusicSliderValue;
    public float PlayerPanelsValue;
    public MenuSaveData(float soundSliderValue, float musicSliderValue, float playerPanelsValue)
    {
        SoundSliderValue = soundSliderValue;
        MusicSliderValue = musicSliderValue;
        PlayerPanelsValue = playerPanelsValue; 
    }
}
