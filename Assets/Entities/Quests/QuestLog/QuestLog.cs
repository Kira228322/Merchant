using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _activeQuestsContent;
    [SerializeField] private VerticalLayoutGroup _finishedQuestsContent;
    [SerializeField] private GameObject _questPanelPrefab;

    private List<QuestPanel> _activeQuests = new();
    private List<QuestPanel> _finishedQuests = new();

    public void AddQuest(Quest quest)
    {
        quest.QuestChangedState += OnQuestChangedState;
        quest.QuestUpdated += OnQuestUpdated;

        if (quest.CurrentState == Quest.State.Active)
        {
            AddToActiveQuests(quest);
        }
        else AddToFinishedQuests(quest);
    }
    private void AddToActiveQuests(Quest quest)
    {
        QuestPanel questPanel = Instantiate(_questPanelPrefab, _activeQuestsContent.transform).GetComponent<QuestPanel>();
        questPanel.Initialize(quest);
        _activeQuests.Add(questPanel);
    }
    private void AddToFinishedQuests(Quest quest)
    {
        QuestPanel questPanel = Instantiate(_questPanelPrefab, _finishedQuestsContent.transform).GetComponent<QuestPanel>();
        questPanel.Initialize(quest);
        _finishedQuests.Add(questPanel);
    }
    private void OnQuestChangedState(Quest quest)
    {
        Destroy(quest.questPanel.gameObject);

        switch (quest.CurrentState)
        {
            case Quest.State.Active:
                AddToActiveQuests(quest);
                break;

            case Quest.State.RewardUncollected:
                if (quest.NextQuestParams != null)
                    QuestHandler.AddQuest(quest.NextQuestParams);
                AddToFinishedQuests(quest);
                break;

            case Quest.State.Completed:
                if (quest.NextQuestParams == null || quest.HasRewards())
                {
                    AddToFinishedQuests(quest);
                }
                else QuestHandler.AddQuest(quest.NextQuestParams);
                break;

            case Quest.State.Failed:
                AddToFinishedQuests(quest);
                break;
        }
    }
    private void OnQuestUpdated(Quest quest)
    {
        quest.questPanel.Refresh();
    }

}