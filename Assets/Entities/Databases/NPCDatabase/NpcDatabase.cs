using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcDatabase : MonoBehaviour, ISaveable<NpcDatabaseSaveData>
{
    public NpcDatabaseSO NpcDatabaseSO;

    public static NpcDatabase Instance;

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

    private void Start()
    {
        //TODO убрать проверку на уникальность айди когда добавим всех нпс
        var duplicateGroups = NpcDatabaseSO.NpcList.GroupBy(npc => npc.ID).Where(group => group.Count() > 1);
        foreach (var group in duplicateGroups)
        {
            Debug.LogWarning($"ID {group.Key} повторяется у следующих НПС:");
            foreach (var npc in group)
            {
                Debug.LogWarning($"- {npc.Name}");
            }
        }
    }

    public static NpcData GetNPCData(int ID)
    {
        NpcData result = Instance.NpcDatabaseSO.NpcList.FirstOrDefault(npc => npc.ID == ID);

        if (result != null) return result;

        Debug.LogWarning($"НПС с таким айди ({ID}) не существует!");
        return null;
    }
    
    public static NpcData GetNPCData(string name)
    {
        NpcData result = Instance.NpcDatabaseSO.NpcList.FirstOrDefault(npc => npc.Name == name);

        if (result != null) return result;

        Debug.LogWarning($"НПС с таким именем ({name}) не существует!");
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

        foreach (NpcData npcData in NpcDatabaseSO.NpcList)
        {
            NpcSaveData npcSaveData;
            switch (npcData)
            {
                case NpcTraderData traderData:
                    npcSaveData = ((ISaveable<NpcTraderSaveData>)traderData).SaveData();
                    break;
                case NpcQuestGiverData questGiverData:
                    npcSaveData = ((ISaveable<NpcQuestGiverSaveData>)questGiverData).SaveData();
                    break;
                default:
                    npcSaveData = npcData.SaveData();
                    break;
            }
            saveData.savedNpcDatas.Add(npcSaveData);
        }

        return saveData;

    }

    public void LoadData(NpcDatabaseSaveData data)
    {
        var npcDataAndSaveData = NpcDatabaseSO.NpcList.Join(data.savedNpcDatas,
        npcData => npcData.ID, savedNpcData => savedNpcData.ID,
        (npcData, savedNpcData) => new { npcData, savedNpcData });
        //^^ This code first creates a variable called npcDataAndSaveData using the Join method to
        // match the NPCData objects with their corresponding NpcSaveData objects based on the ID property.
        // The result is an enumerable of anonymous types containing both the NPCData object and the
        // matching NpcSaveData object.

        foreach (var npcAndSaveData in npcDataAndSaveData)
        {
            switch (npcAndSaveData.npcData)
            {
                case NpcTraderData npcTraderData:
                    ((ISaveable<NpcTraderSaveData>)npcTraderData).LoadData((NpcTraderSaveData)npcAndSaveData.savedNpcData);
                    break;
                case NpcQuestGiverData npcQuestGiverData:
                    ((ISaveable<NpcQuestGiverSaveData>)npcQuestGiverData).LoadData((NpcQuestGiverSaveData)npcAndSaveData.savedNpcData);
                    break;
                default:
                    npcAndSaveData.npcData.LoadData(npcAndSaveData.savedNpcData);
                    break;
            }
        }
    }
}
