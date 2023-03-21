using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Database", menuName = "Databases/NPC Database")]
public class NpcDatabaseSO : ScriptableObject
{
    public List<NpcData> NpcList;
}
