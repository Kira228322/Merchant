using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;

[Serializable]
public class TutorialStateTrackerSaveData
{
    public Dictionary<TutorialStateTracker.PresentationInfo, bool> SavedDictionary;
    public List<TutorialStateTracker.PresentationInfo> PresentationInfos;
    public List<bool> Bools;
    public TutorialStateTrackerSaveData(Dictionary<TutorialStateTracker.PresentationInfo, bool> dictionary)
    {
        SavedDictionary = dictionary;
        foreach (var el in dictionary)
        {
            PresentationInfos.Add(el.Key);
            Bools.Add(el.Value);
        }
    }
}
