using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Database", menuName = "Databases/NPC Database")]
public class NPCDatabaseSO : ScriptableObject
{
    public List<NPCData> NPCList;
}
