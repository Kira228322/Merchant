using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newQuestLine", menuName = "Quest/Quest Line")]
public class QuestLine : ScriptableObject
{
    public string QuestLineName;
    public List<PregenQuestSO> QuestsInLine = new();

    public bool IsLast(PregenQuestSO pregenQuest)
    {
        return QuestsInLine.IndexOf(pregenQuest) == QuestsInLine.Count - 1;
    }

    protected virtual void OnEnable()
    {
        foreach (PregenQuestSO pregenQuestSO in QuestsInLine)
        {
            if (pregenQuestSO.QuestLine != null && pregenQuestSO.QuestLine != this)
            {
                Debug.LogError($" вест {pregenQuestSO.QuestName} одновременно принадлежит к двум цепочкам: {QuestLineName} и {pregenQuestSO.QuestLine.QuestLineName}");
            }
            pregenQuestSO.QuestLine = this;
        }
    }
}
