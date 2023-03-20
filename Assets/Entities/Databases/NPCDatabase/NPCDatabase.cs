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

        Debug.LogWarning("НПС с таким именем не существует!");
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

        foreach (NPCData npcData in NpcDatabaseSO.NPCList)
        {
            NpcSaveData npcSaveData;
            if (npcData is NpcTraderData traderData) //TODO switch для каждого типа сохраняемого NPC или Dictionary 
            {
                npcSaveData = ((ISaveable<NpcTraderSaveData>)traderData).SaveData();
            }
            else
            {
                npcSaveData = npcData.SaveData();
            }
            saveData.savedNpcDatas.Add(npcSaveData);
        }

        return saveData;

    }

    public void LoadData(NpcDatabaseSaveData data)
    {
        var npcDataAndSaveData = NpcDatabaseSO.NPCList.Join(data.savedNpcDatas,
        npcData => npcData.ID, savedNpcData => savedNpcData.ID,
        (npcData, savedNpcData) => new { npcData, savedNpcData });
        //^^ This code first creates a variable called npcDataAndSaveData using the Join method to
        // match the NPCData objects with their corresponding NpcSaveData objects based on the ID property.
        // The result is an enumerable of anonymous types containing both the NPCData object and the
        // matching NpcSaveData object.

        foreach (var npcAndSaveData in npcDataAndSaveData)
        {
            Debug.Log(npcAndSaveData.savedNpcData.ID);
            if (npcAndSaveData.npcData is NpcTraderData)
            {
                ((ISaveable<NpcTraderSaveData>)npcAndSaveData.npcData).LoadData((NpcTraderSaveData)npcAndSaveData.savedNpcData);
            }
            else
            {
                npcAndSaveData.npcData.LoadData(npcAndSaveData.savedNpcData);
            }
        }
    }
}
