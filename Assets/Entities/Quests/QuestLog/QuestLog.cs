using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _activeQuestsContent;
    [SerializeField] private VerticalLayoutGroup _finishedQuestsContent;
    [SerializeField] private GameObject _questPanelPrefab;
    [SerializeField] private GameObject _questLinePanelPrefab;
    [SerializeField] private MiscQuestLine _miscQuestLineActive;
    [SerializeField] private MiscQuestLine _miscQuestLineFinished;

    [SerializeField] private QuestComplete _questCompleteAnnouncer;

    private List<QuestPanel> _activeQuests = new();
    private List<QuestPanel> _finishedQuests = new();

    private Dictionary<QuestLine, QuestLinePanel> _activeQuestLines = new();
    private Dictionary<QuestLine, QuestLinePanel> _finishedQuestLines = new();

    private class OrderedQuestLinePanel
    {
        public QuestLinePanel QuestLinePanel;
        public int MostRecentQuestValue;
        public bool ContainsRewardUncollected;

        public OrderedQuestLinePanel(QuestLinePanel panel, int recentValue, bool rewardUncollected)
        {
            QuestLinePanel = panel;
            MostRecentQuestValue = recentValue;
            ContainsRewardUncollected = rewardUncollected;
        }
    }

    public void AddQuest(Quest quest)
    {
        quest.QuestChangedState += OnQuestChangedState;
        quest.QuestUpdated += OnQuestUpdated;

        CheckQuestLineAndAddNewPanel(quest, quest.CurrentState, true);
    }
    private void AddToQuestList(Quest quest, QuestLine questLine, Dictionary<QuestLine, QuestLinePanel> questLines, List<QuestPanel> quests, Transform contentTransform, QuestLine miscQuestLine)
    {
        QuestLinePanel targetQuestLinePanel;
        if (questLine != null)
        {
            if (!questLines.ContainsKey(questLine))
            {
                targetQuestLinePanel = Instantiate(_questLinePanelPrefab, contentTransform).GetComponent<QuestLinePanel>();
                questLines.Add(questLine, targetQuestLinePanel);
                targetQuestLinePanel.Initialize(questLine);
            }
            else
            {
                targetQuestLinePanel = questLines[questLine];
            }
        }
        else
        {
            if (!questLines.ContainsKey(miscQuestLine))
            {
                targetQuestLinePanel = Instantiate(_questLinePanelPrefab, contentTransform).GetComponent<QuestLinePanel>();
                questLines.Add(miscQuestLine, targetQuestLinePanel);
                miscQuestLine.QuestsInLine.Add(PregenQuestDatabase.GetPregenQuest(quest.QuestSummary));
                targetQuestLinePanel.Initialize(miscQuestLine);
            }
            else
            {
                targetQuestLinePanel = questLines[miscQuestLine];
            }
        }

        QuestPanel questPanel = Instantiate(_questPanelPrefab, targetQuestLinePanel.ItemContentTransform).GetComponent<QuestPanel>();
        questPanel.Initialize(quest);
        targetQuestLinePanel.Refresh();
        quests.Add(questPanel);
    }

    private void AddToActiveQuests(Quest quest, QuestLine questLine)
    {
        AddToQuestList(quest, questLine, _activeQuestLines, _activeQuests, _activeQuestsContent.transform, _miscQuestLineActive);
    }

    private void AddToFinishedQuests(Quest quest, QuestLine questLine)
    {
        AddToQuestList(quest, questLine, _finishedQuestLines, _finishedQuests, _finishedQuestsContent.transform, _miscQuestLineFinished);
    }

    private void OnQuestChangedState(Quest quest, Quest.State oldState, Quest.State newState)
    {
        //� ����� ��� � �������, ����� ������� ����� ������ ��� ������� � ���������� ��� � ������� ������� �������
        Destroy(quest.questPanel.gameObject);
        quest.questPanel.transform.SetParent(null); // Destroy �� ����������� �����, � QuestLinePanel ����� ������� ��� ��� ������ �� ����� ������, ��� ������ ����������� ����������� ������� ����� (29.11.23)
        CheckQuestLineAndAddNewPanel(quest, newState, false);
  
    }
    private void CheckQuestLineAndAddNewPanel(Quest quest, Quest.State newState, bool newOrLoading)
    {
        //�����: �������� ����� Quest quest.QuestSummary PregenQuestSO ����� ���� ������. � ���� �� ����� ��� QuestLine, ����� ������ �� ��� ���������.
        //���� ��������� � �� �����������, �� ������ �������� ���������� �������.
        //���� ������ � �� �����������, �� ��������� �������.
        //���� ����������� �� ������, ��������� ��� � ������������ �������. (��� ���� ����������� ������ ���������� - ���� ������ ��� ��������?)
        //(�� ���� ���� ������� ���, ��������� � �� �����)

        //�� ����� ���� ������, ����� ��������� �����, ����� ������, � ���� �� ��� ����� �������.
        //���� ����, ��������� �. ���� ����� ������� � �� ��������� � �������, �� � ����� �������.
        //���� �� ������� ��� ����������� �������, ��� �� ��������� �������.

        PregenQuestSO pregenQuest = PregenQuestDatabase.GetPregenQuest(quest.QuestSummary);
        QuestLine questLine = pregenQuest.QuestLine;

        if (questLine != null)
        {
            //������ ���� isLast, ����� ������� �������, ������ ���� ���� ������� � ������� ��������� � ����� ������ ���� ���� �������.

            bool isLast = questLine.IsLast(pregenQuest);

            switch (newState)
            {
                case Quest.State.Active:
                    AddToActiveQuests(quest, questLine);
                    break;
                case Quest.State.RewardUncollected:
                    if (isLast && !newOrLoading)
                    {
                        _questCompleteAnnouncer.ChangeText(questLine.QuestLineName);
                    }
                    AddToFinishedQuests(quest, questLine);
                    break;
                case Quest.State.Completed:
                    if (isLast && !quest.HasRewards() && !newOrLoading)
                    {
                        _questCompleteAnnouncer.ChangeText(questLine.QuestLineName);
                    }
                    AddToFinishedQuests(quest, questLine);
                    break;
                case Quest.State.Failed:
                    AddToFinishedQuests(quest, questLine);
                    break;
            }
            if (!newOrLoading && _activeQuestLines.TryGetValue(questLine, out QuestLinePanel activeQuestLinePanel))
            {
                if (isLast)
                {
                    //���� ����� ��������� � ������� � ��� ���� ������� QuestLinePanel (� ��� �� ����� �������, ���� ��� ��������) �� ������� �.
                    //����� ������� �� ���������� �������� �� ������������, � ����������� ������� ������� �� ������������.
                    Destroy(activeQuestLinePanel.gameObject);
                    _activeQuestLines.Remove(questLine);
                }
                else
                {
                    //12.01.24: � ����� ������ ���� ��������� ������: ���� ����� �����������, � ��������� ����� ��������� � ���������.
                    //�������� ������� ��� �� ����, �� ������ �� ���������. � ���� ������� ������������ ������ ���:
                    //��� ��� �������, ������� �� ���� ���� ����� �� ���� �������. ���� ���, �� ������ ������� ����� ������.

                    //14.01.24: ��������� ���� �� ��������, ������ ��� ������ ����� ��������� �� ������� Quest.QuestChangedState.
                    //� ���� ������ �� ������� ����� ����� �� �������, ������� � ActiveQuests � QuestHandler.
                    //�� QuestHandler ���� ��������� �� ������� Quest.QuestChangedState => ����� ��� ����,
                    //��� �����, �����������, ��� �� ������� �� ActiveQuests. ������� �������� ������� �������: ����� �����,
                    //����� ��� ��� ������ ��� ���������� ���������, ��� �� ������������ �� QuestHandler.

                    bool shouldDestroyPanel = true;
                    foreach (var questInLine in questLine.QuestsInLine)
                    {
                        Quest foundQuest = QuestHandler.GetActiveQuestBySummary(questInLine.QuestSummary);
                        if (foundQuest?.CurrentState == Quest.State.Active)
                        {
                            shouldDestroyPanel = false;
                        }
                    }
                    if (shouldDestroyPanel)
                    {
                        Destroy(activeQuestLinePanel.gameObject);
                        _activeQuestLines.Remove(questLine);
                    }
                }
            }
        }
        else
        {
            //������ ���� � "������" ������ ��� ������� � ������� ��� �������. ������ ���� ���� ������� � ������� ��������� � ������ ���� ������ ������
            switch (newState)
            {
                case Quest.State.Active:
                    AddToActiveQuests(quest, questLine);
                    break;
                case Quest.State.RewardUncollected:
                    _miscQuestLineActive.QuestsInLine.Remove(pregenQuest);
                    if (!newOrLoading)
                    {
                        _questCompleteAnnouncer.ChangeText(quest.QuestName);
                    }
                    AddToFinishedQuests(quest, questLine);
                    break;
                case Quest.State.Completed:
                    if (!quest.HasRewards() && !newOrLoading)
                    {
                        _miscQuestLineActive.QuestsInLine.Remove(pregenQuest);
                        _questCompleteAnnouncer.ChangeText(quest.QuestName);
                    }
                    AddToFinishedQuests(quest, questLine);
                    break;

                case Quest.State.Failed:
                    if (!newOrLoading)
                    {
                        _miscQuestLineActive.QuestsInLine.Remove(pregenQuest);
                    }
                    AddToFinishedQuests(quest, questLine);
                    break;
            }

            if (!newOrLoading && _miscQuestLineActive.QuestsInLine.Count == 0 && _activeQuestLines.TryGetValue(_miscQuestLineActive, out QuestLinePanel activeQuestLinePanel))
            {
                Destroy(activeQuestLinePanel.gameObject);
                _activeQuestLines.Remove(_miscQuestLineActive);
            }
        }

        SortPanels(newState == Quest.State.Active);
    }
    private void OnQuestUpdated(Quest quest)
    {
        quest.questPanel.Refresh();
        
    }

    private void SortPanels(bool activePanels)
    {
        if (activePanels)
        {
            List<OrderedQuestLinePanel> orderedQuestLinePanels = _activeQuestLines
                .Select(questLinePanel =>
                {
                    List<QuestPanel> children = questLinePanel.Value.GetComponentsInChildren<QuestPanel>().ToList();

                    if (children.Count == 0)
                        return null;

                    bool containsRewardUncollected = children.Any(child => child.Quest.CurrentState == Quest.State.RewardUncollected);
                    children = children.OrderBy(questPanel => questPanel.Quest.DayStartedOn * 24 + questPanel.Quest.HourStartedOn).ToList();

                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].transform.SetSiblingIndex(i);
                    }

                    return new OrderedQuestLinePanel(questLinePanel.Value, children[0].Quest.DayStartedOn * children[0].Quest.HourStartedOn, containsRewardUncollected);
                })
                    .Where(panel => panel != null)
                    .OrderBy(orderedPanel => orderedPanel.ContainsRewardUncollected)
                    .ThenBy(orderedPanel => orderedPanel.MostRecentQuestValue)
                    .ToList();

            for (int i = 0; i < orderedQuestLinePanels.Count; i++)
            {
                orderedQuestLinePanels[i].QuestLinePanel.transform.SetSiblingIndex(i);
            }
        }
        else 
        {
            List<OrderedQuestLinePanel> orderedQuestLinePanels = _finishedQuestLines
                .Select(questLinePanel =>
                {
                    List<QuestPanel> children = questLinePanel.Value.GetComponentsInChildren<QuestPanel>().ToList();

                    if (children.Count == 0)
                        return null;

                    bool containsRewardUncollected = children.Any(child => child.Quest.CurrentState == Quest.State.RewardUncollected);
                    children = children.OrderBy(questPanel => questPanel.Quest.DayStartedOn * 24 + questPanel.Quest.HourStartedOn).ToList();

                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].transform.SetSiblingIndex(i);
                    }

                    return new OrderedQuestLinePanel(questLinePanel.Value, children[0].Quest.DayStartedOn * children[0].Quest.HourStartedOn, containsRewardUncollected);
                })
                    .Where(panel => panel != null)
                    .OrderBy(orderedPanel => orderedPanel.ContainsRewardUncollected)
                    .ThenBy(orderedPanel => orderedPanel.MostRecentQuestValue)
                    .ToList();

            for (int i = 0; i < orderedQuestLinePanels.Count; i++)
            {
                orderedQuestLinePanels[i].QuestLinePanel.transform.SetSiblingIndex(i);
            }
        }
    }

}