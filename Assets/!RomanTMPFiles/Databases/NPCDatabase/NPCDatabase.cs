using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCDatabase : MonoBehaviour
{
    public NPCDatabaseSO NPCs;
    private static NPCDatabase Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
        }
    }

    public static NPCData GetNPC(int ID)
    {
        NPCData result = Singleton.NPCs.NPCList.FirstOrDefault(npc => npc.ID == ID);

        if (result != null) return result;

        Debug.LogWarning("НПС с таким айди не существует!");
        return null;
    }
    public static NPCData GetNPCData(string name)
    {
        NPCData result = Singleton.NPCs.NPCList.FirstOrDefault(npc => npc.Name == name);

        if (result != null) return result;

        Debug.LogWarning("НПС с таким айди не существует!");
        return null;
    }
}
