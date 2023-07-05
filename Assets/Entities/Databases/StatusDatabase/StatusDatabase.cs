using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusDatabase : MonoBehaviour
{
    public StatusDatabaseSO Statuses;
    public static StatusDatabase Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static Status GetStatus(string name)
    {
        Status result = Instance.Statuses.StatusList.FirstOrDefault(status => status.StatusName.ToLower() == name.ToLower());
        if (result != null)
        {
            return result;
        }

        Debug.LogWarning("Такого статуса не существует!");
        return null;
    }
}
