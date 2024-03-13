using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CooldownHandler : MonoBehaviour, ISaveable<CooldownHandlerSaveData>
{
    [System.Serializable]
    public class ObjectOnCooldown
    {
        public string UniqueID;
        private int _hoursLeft;
        public int HoursLeft
        {
            get => _hoursLeft;
            set
            {
                _hoursLeft = value;
                if (_hoursLeft <= 0)
                {
                    _hoursLeft = 0;
                }
            }
        }
        public ObjectOnCooldown(string id, int hoursLeft)
        {
            UniqueID = id;
            HoursLeft = hoursLeft;
        }
    }

    [HideInInspector] public List<ObjectOnCooldown> ObjectsOnCooldown = new();
    public event UnityAction<string> ReadyToReset;
    private void OnEnable()
    {
        GameTime.HourChanged += OnHourChanged;
        GameTime.TimeSkipped += OnTimeSkipped;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
        GameTime.TimeSkipped -= OnTimeSkipped;
    }

    public void Register(string uniqueID, int cooldownHours)
    {
        if (!ObjectsOnCooldown.Any(item => item.UniqueID == uniqueID))
            ObjectsOnCooldown.Add(new(uniqueID, cooldownHours));
    }
    public void Unregister(string uniqueID)
    {
        ObjectsOnCooldown.Remove(ObjectsOnCooldown.FirstOrDefault(item => item.UniqueID == uniqueID));
    }

    private void OnHourChanged()
    {
        foreach (ObjectOnCooldown objectOnCooldown in ObjectsOnCooldown.ToList())
        {
            objectOnCooldown.HoursLeft--;
            if (objectOnCooldown.HoursLeft <= 0)
            {
                ReadyToReset?.Invoke(objectOnCooldown.UniqueID);
            }
        }
    }
    private void OnTimeSkipped(int skippedDays, int skippedHours, int skippedMinutes)
    {
        int totalSkippedHours = skippedDays * 24 + skippedHours + skippedMinutes / 60;
        foreach (ObjectOnCooldown objectOnCooldown in ObjectsOnCooldown.ToList())
        {
            objectOnCooldown.HoursLeft -= totalSkippedHours;
            if (objectOnCooldown.HoursLeft <= 0)
            {
                ReadyToReset?.Invoke(objectOnCooldown.UniqueID);
            }
        }
    }

    public CooldownHandlerSaveData SaveData()
    {
        CooldownHandlerSaveData saveData = new(this);
        return saveData;
    }

    public void LoadData(CooldownHandlerSaveData data)
    {
        ObjectsOnCooldown.AddRange(data.ObjectsOnCooldown);
    }
}
