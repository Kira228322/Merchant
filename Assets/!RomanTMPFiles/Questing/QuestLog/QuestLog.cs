using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _activeQuestsContent;
    [SerializeField] private VerticalLayoutGroup _completedQuestsContent;
    [SerializeField] private GameObject _questPanelPrefab;

    private List<QuestPanel> _activeQuests = new();
    private List<QuestPanel> _completedQuests = new();
    public void AddToActiveQuests(Quest quest)
    {
        QuestPanel questPanel = Instantiate(_questPanelPrefab, _activeQuestsContent.transform).GetComponent<QuestPanel>();
        questPanel.Initialize(quest);
        _activeQuests.Add(questPanel);
    }
    public void MoveToCompletedQuests(Quest quest)
    {
        QuestPanel questPanel = quest.questPanel;
        CopyToCompleteQuests(questPanel);
        _activeQuests.Remove(questPanel);
        Destroy(questPanel.gameObject);
    }
    public void CopyToCompleteQuests(QuestPanel questPanel)
    {
        Instantiate(questPanel.gameObject, _completedQuestsContent.transform);
        //questPanel.Initialize(questPanel.Quest);

        _completedQuests.Add(questPanel);
    }
    private QuestPanel GetPanelFromActiveQuest(Quest quest)
    {
        return _activeQuests.FirstOrDefault(q => q.Quest == quest);
    }
}
