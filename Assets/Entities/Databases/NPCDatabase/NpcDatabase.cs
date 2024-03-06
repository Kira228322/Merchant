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
            NpcSaveData npcSaveData = npcData switch
            {
                NpcTraderData traderData => ((ISaveable<NpcTraderSaveData>)traderData).SaveData(),
                NpcQuestGiverData questGiverData => ((ISaveable<NpcQuestGiverSaveData>)questGiverData).SaveData(),
                _ => npcData.SaveData(),
            };
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
