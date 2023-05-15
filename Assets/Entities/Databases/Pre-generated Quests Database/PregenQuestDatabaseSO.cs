using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pregen Quest Database", menuName = "Databases/Pregenerated Quest Database")]
public class PregenQuestDatabaseSO : ScriptableObject
{
    [Tooltip("Не рандомные квесты, например сюжетные")] public List<PregenQuestSO> ScriptedQuests;
    [Tooltip("Рандомные квесты для доски объявлений")] public List<RandomQuestSO> RandomQuests;
    
    [Serializable] public class RandomQuestSO
    {
        public PregenQuestSO RandomQuest;
        public string Name;
        public string Description;
    }
}
