using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCDatabase : MonoBehaviour, ISaveable<NpcDatabaseSaveData>
{
    public NPCDatabaseSO NpcDatabaseSO;

    private static NPCDatabase Instance;

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

    public static NPCData GetNPCData(int ID)
    {
        NPCData result = Instance.NpcDatabaseSO.NPCList.FirstOrDefault(npc => npc.ID == ID);

        if (result != null) return result;

        Debug.LogWarning("НПС с таким айди не существует!");
        return null;
    }
    public static NPCData GetNPCData(string name)
    {
        NPCData result = Instance.NpcDatabaseSO.NPCList.FirstOrDefault(npc => npc.Name == name);

        if (result != null) return result;

        Debug.LogWarning("НПС с таким айди не существует!");
        return null;
    }

    public static NpcDatabaseSaveData SaveNPCs()
    {
        return Instance.SaveData();
    }

    public static void LoadNPCs(NpcDatabaseSaveData data)
    {
        Instance.LoadData(data);
    }

    public NpcDatabaseSaveData SaveData()
    {
        NpcDatabaseSaveData saveData = new();

        //TODO 18.03

        return saveData;

    }

    public void LoadData(NpcDatabaseSaveData data)
    {
        //TODO 18.03
    }
}
