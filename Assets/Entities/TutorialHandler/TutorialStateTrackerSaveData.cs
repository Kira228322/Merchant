using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialStateTrackerSaveData
{
    public Dictionary<TutorialStateTracker.PresentationInfo, bool> SavedDictionary;

    public TutorialStateTrackerSaveData(Dictionary<TutorialStateTracker.PresentationInfo, bool> dictionary)
    {
        SavedDictionary = dictionary;
    }
}
