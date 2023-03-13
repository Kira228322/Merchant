using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WagonPartDatabase : MonoBehaviour
{
    public WagonPartDatabaseSO WagonParts;
    private static WagonPartDatabase Instance;

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

    public static WagonPart GetWagonPart(string name)
    {
        WagonPart result = Instance.WagonParts.WagonPartsList.FirstOrDefault(wagonPart => wagonPart.Name == name);
 
        if (result != null)
        {
            return result;
        }

        Debug.LogWarning("Такого WagonPart не существует!");
        return null;
    }

    public static WagonPart GetWagonPartByLvl<T>(int lvl)
    {
        WagonPart result = Instance.WagonParts.WagonPartsList.FirstOrDefault(wagonPart => wagonPart is T && wagonPart.Level == lvl);
        
        if (result != null)
        {
            return result;
        }

        Debug.LogWarning("Такого WagonPart не существует!");
        return null;
    }
}
