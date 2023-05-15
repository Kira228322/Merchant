using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pregen Quest Database", menuName = "Databases/Pregenerated Quest Database")]
public class PregenQuestDatabaseSO : ScriptableObject
{
    [Tooltip("�� ��������� ������, �������� ��������")] public List<PregenQuestSO> ScriptedQuests;
    [Tooltip("��������� ������ ��� ����� ����������")] public List<RandomQuestSO> RandomQuests;
    
    [Serializable] public class RandomQuestSO
    {
        public PregenQuestSO RandomQuest;
        public string Name;
        public string Description;
    }
}
