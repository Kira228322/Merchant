using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusManager : MonoBehaviour, ISaveable<StatusManagerSaveData>
{
    [HideInInspector] public static StatusManager Instance;

    [SerializeField] private GameObject _statusPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private Status _lowNeedsDebuff;

    [HideInInspector] public List<ActiveStatus> ActiveStatuses = new();
    public GameObject CurrentStatusInfoWindow;

    public ActiveStatus AddStatusForPlayer(Status status)
    {
        ActiveStatus alreadyActive = ActiveStatuses.FirstOrDefault(s => s.StatusData.StatusName == status.StatusName);
        if (alreadyActive != null)
        {
            alreadyActive.RefreshStatus(status);
            return alreadyActive;
        }
        ActiveStatus newStatus = new();
        newStatus.Init(status);
        Instantiate(_statusPrefab, _container).GetComponent<StatusUIObject>().Init(newStatus);
        ActiveStatuses.Add(newStatus);
        return newStatus;
    }
    private void DecreaseDuration()
    {
        for (int i = ActiveStatuses.Count - 1; i >= 0; i--)
        {
            ActiveStatus status = ActiveStatuses[i];
            status.DecreaseDuration();
            if (!status.IsActive)
            {
                ActiveStatuses.RemoveAt(i);
            }
        }
    }
    private void OnTimeSkipped(int daysAdded, int hoursAdded, int minutesAdded)
    {
        float hoursSkipped = daysAdded * 24 + hoursAdded + ((float)minutesAdded / 60);
        for (int i = ActiveStatuses.Count - 1; i >= 0; i--)
        {
            ActiveStatus status = ActiveStatuses[i];
            status.DecreaseDuration(hoursSkipped);
            if (!status.IsActive)
            {
                ActiveStatuses.RemoveAt(i);
            }
        }
    }

    public void AddLowNeedsDebuff()
    {
        AddStatusForPlayer(_lowNeedsDebuff);
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        GameTime.MinuteChanged += DecreaseDuration;
        GameTime.TimeSkipped += OnTimeSkipped;
    }
    private void OnDestroy()
    {
        GameTime.MinuteChanged -= DecreaseDuration;
        GameTime.TimeSkipped -= OnTimeSkipped;
    }
    public StatusManagerSaveData SaveData()
    {
        StatusManagerSaveData saveData = new(ActiveStatuses);
        return saveData;
    }

    public void LoadData(StatusManagerSaveData data)
    {
        foreach (var savedStatus in data.savedActiveStatuses)
        {
            ActiveStatus loadedStatus = AddStatusForPlayer(StatusDatabase.GetStatus(savedStatus.statusDataName));
            loadedStatus.SetDuration(savedStatus.currentDurationHours);
        }
    }
}
