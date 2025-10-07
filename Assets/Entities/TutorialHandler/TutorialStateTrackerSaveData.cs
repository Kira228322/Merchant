using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;

[Serializable]
public class TutorialStateTrackerSaveData
{
    public List<TutorialStateTracker.PresentationInfo> PresentationInfos = new ();
    public List<bool> Bools = new ();
    public TutorialStateTrackerSaveData(Dictionary<TutorialStateTracker.PresentationInfo, bool> dictionary)
    {
        foreach (var el in dictionary)
        { 
            PresentationInfos.Add(el.Key);
            Bools.Add(el.Value);
        }
    }

    public Dictionary<TutorialStateTracker.PresentationInfo, bool> GetDictionary()
    {
        Dictionary<TutorialStateTracker.PresentationInfo, bool> dictionary =
            new Dictionary<TutorialStateTracker.PresentationInfo, bool>();
        for (int i = 0; i < PresentationInfos.Count; i++)
        {
            dictionary.Add(PresentationInfos[i], Bools[i]);
        }

        return dictionary;
    }
}
