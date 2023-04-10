using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CooldownHandler: MonoBehaviour, ISaveable<CooldownHandlerSaveData>
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

    public List<ObjectOnCooldown> ObjectsOnCooldown = new();
    public event UnityAction<string> ReadyToReset;
    private void OnEnable()
    {
        GameTime.HourChanged += OnHourChanged;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
    }

    public void Register(string uniqueID, int cooldownHours)
    {
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
            if (objectOnCooldown.HoursLeft == 0)
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
