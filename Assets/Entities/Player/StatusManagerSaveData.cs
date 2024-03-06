using System;
using System.Collections.Generic;

[Serializable]
public class StatusManagerSaveData
{
    [Serializable]
    public class SavedActiveStatus
    {
        public string statusDataName;
        public float currentDurationHours;
        public SavedActiveStatus(ActiveStatus status)
        {
            statusDataName = status.StatusData.StatusName;
            currentDurationHours = status.CurrentDurationHours;
        }
    }

    public List<SavedActiveStatus> savedActiveStatuses;

    public StatusManagerSaveData(List<ActiveStatus> activeStatuses)
    {
        savedActiveStatuses = new();
        foreach (var status in activeStatuses)
        {
            SavedActiveStatus savedStatus = new(status);
            savedActiveStatuses.Add(savedStatus);
        }
    }
}
