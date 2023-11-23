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

    private Dictionary<QuestLine, QuestLinePanel> _activeQuestLines = new();
    private Dictionary<QuestLine, QuestLinePanel> _finishedQuestLines = new();

    public void AddQuest(Quest quest)
    {
        quest.QuestChangedState += OnQuestChangedState;
        quest.QuestUpdated += OnQuestUpdated;

        CheckQuestLineAndAddNewPanel(quest, quest.CurrentState);
    }
    private void AddToActiveQuests(Quest quest, QuestLine questLine)
    {
        //TODO: ���������� �� ���� ������ ����� ���������� ����� �������

        Transform targetTransform = null;

        if (questLine != null)
        {
            if (!_activeQuestLines.ContainsKey(questLine))
            {
                // QuestLinePanel newQuestLinePanel = Instantiate(_questLinePanelPrefab, _activeQuestsContent.transform).GetComponent<QuestLinePanel>();
                //_activeQuestLines.Add(questLine, newQuestLinePanel);
                // newQuestLinePanel.Initialize(questLine)
                // targetTransform = newQuestLinePanel.transform;
            }
            else
            {
                //targetTransform = _activeQuestLines[questLine].transform;
            }
        }
        else //������ ��� ������� ����� ������������ � ������� "������". ��� ����-�������
        {
            if (_activeQuestLines[null] == null) //[null] ����� ��������������� "������"
            {
                // QuestLinePanel newQuestLinePanel = Instantiate(_questLinePanelPrefab, _activeQuestsContent.transform).GetComponent<QuestLinePanel>();
                // _activeQuestLines[null] = newQuestLinePanel;
                // newQuestLinePanel.Initialize(null);
                // targetTransform = newQuestLinePanel.transform;
            }
            else
            {
                //targetTransform = _activeQuestLines[null].transform;
            }
        }
        QuestPanel questPanel = Instantiate(_questPanelPrefab, targetTransform).GetComponent<QuestPanel>();
        questPanel.Initialize(quest);
        _activeQuests.Add(questPanel);
    }
    private void AddToFinishedQuests(Quest quest, QuestLine questLine)
    {
        Transform targetTransform = null;

        if (questLine != null)
        {
            if (!_activeQuestLines.ContainsKey(questLine))
            {
                // QuestLinePanel newQuestLinePanel = Instantiate(_questLinePanelPrefab, _activeQuestsContent.transform).GetComponent<QuestLinePanel>();
                // _activeQuestLines.Add(questLine, newQuestLinePanel);
                // newQuestLinePanel.Initialize(questLine)
                // targetTransform = newQuestLinePanel.transform;
            }
            else
            {
                //targetTransform = _activeQuestLines[questLine].transform;
            }
        }
        else //������ ��� ������� ����� ������������ � ������� "������". ��� ����-�������
        {
            if (_activeQuestLines[null] == null) //[null] ����� ��������������� "������"
            {
                // QuestLinePanel newQuestLinePanel = Instantiate(_questLinePanelPrefab, _activeQuestsContent.transform).GetComponent<QuestLinePanel>();
                // _activeQuestLines[null] = newQuestLinePanel;
                // newQuestLinePanel.Initialize(null);
                // targetTransform = newQuestLinePanel.transform;
            }
            else
            {
                //targetTransform = _activeQuestLines[null].transform;
            }
        }

        QuestPanel questPanel = Instantiate(_questPanelPrefab, targetTransform.transform).GetComponent<QuestPanel>();
        questPanel.Initialize(quest);
        _finishedQuests.Add(questPanel);
    }

    private void OnQuestChangedState(Quest quest, Quest.State oldState, Quest.State newState)
    {
        //� ����� ��� � �������, ����� ������� ����� ������ ��� ������� � ���������� ��� � ������� ������� �������
        Destroy(quest.questPanel.gameObject);
        CheckQuestLineAndAddNewPanel(quest, newState);
  
    }
    private void CheckQuestLineAndAddNewPanel(Quest quest, Quest.State newState)
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
        bool isLast = false;

        if (questLine != null)
        {
            isLast = questLine.IsLast(pregenQuest);
        }

        
        if (newState == Quest.State.Active)
        {
            AddToActiveQuests(quest, questLine);
        }
        else
        {
            //���� ����� ��������� � ������� � ��� ���� ������� QuestLinePanel (� ��� �� ����� �������, ���� ��� ��������) �� ������� �.
            //����� ������� �� ���������� �������� �� ������������, � ����������� ������� ������� �� ������������.
            if (isLast && _activeQuestLines[questLine] != null)
            {
                Destroy(_activeQuestLines[questLine]);
                _activeQuestLines.Remove(questLine);
            }
            AddToFinishedQuests(quest, questLine);
        }

        //TODO ����������� ������ ������� �� ���� ������/���������� ����� ���������� ������?
    }
    private void OnQuestUpdated(Quest quest)
    {
        quest.questPanel.Refresh();
    }

}