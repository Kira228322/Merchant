using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestCheatQuestGiver : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown itemSelector;

    private void Start()
    {
        List<string> questNames = new();
        foreach (PregenQuestSO item in PregenQuestDatabase.QuestList.ScriptedQuests)
        {
            questNames.Add(item.QuestSummary);
        }
        itemSelector.AddOptions(questNames);

    }

    public void AddQuest()
    {
        QuestHandler.AddQuest(PregenQuestDatabase.GetQuestParams(itemSelector.captionText.text));
    }

}
