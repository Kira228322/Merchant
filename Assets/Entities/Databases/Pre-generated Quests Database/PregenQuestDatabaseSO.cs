using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pregen Quest Database", menuName = "Databases/Pregenerated Quest Database")]
public class PregenQuestDatabaseSO : ScriptableObject
{
    public List<PregenQuestSO> PregenQuests;
}
