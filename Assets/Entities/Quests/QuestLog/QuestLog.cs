using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
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

        CheckQuestLineAndAddNewPanel(quest, quest.CurrentState);
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

    //�����: �������� ����� Quest quest.QuestSummary PregenQuestSO ����� ���� ������. � ���� �� ����� ��� QuestLine, ����� ������ �� ��� ���������.
    //���� ��������� � �� �����������, �� ������ �������� ���������� �������.
    //���� ������ � �� �����������, �� ��������� �������.
    //���� ����������� �� ������, ��������� ��� � ������������ �������. (��� ���� ����������� ������ ���������� - ���� ������ ��� ��������?)
    //(�� ���� ���� ������� ���, ��������� � �� �����)

    private void OnQuestChangedState(Quest quest, Quest.State oldState, Quest.State newState)
    {
        Destroy(quest.questPanel.gameObject);
        CheckQuestLineAndAddNewPanel(quest, newState);
  
    }
    private void CheckQuestLineAndAddNewPanel(Quest quest, Quest.State newState)
    {
        PregenQuestSO pregenQuest = PregenQuestDatabase.GetPregenQuest(quest.QuestSummary);
        QuestLine questLine = pregenQuest.QuestLine;
        bool isFirst = false;
        bool isLast = false;

        if (questLine != null)
        {
            isFirst = questLine.IsFirst(pregenQuest);
            isLast = questLine.IsLast(pregenQuest);
        }

        if (newState == Quest.State.Active)
        {
            AddToActiveQuests(quest); //���� ������ �������� ������� �� �������� ����
            if (isFirst)
                Debug.Log("����������� ����� ������ � �������, ������ ����� ������� �");
        }
        else
        {
            AddToFinishedQuests(quest); //���� ������ �������� ������� �� �������� ����
            if (isLast)
                Debug.Log("����������� ����� ��������� � �������, ������ ��� ���������");
        }
    }
    private void OnQuestUpdated(Quest quest)
    {
        quest.questPanel.Refresh();
    }

}