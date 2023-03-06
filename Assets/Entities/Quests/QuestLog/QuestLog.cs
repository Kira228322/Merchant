using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour, ISaveable<QuestsSaveData>
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

        _completedQuests.Add(questPanel);
    }

    public void AddQuestAsFinished(QuestsSaveData.SavedQuestPanel savedPanel)
    {
        //��������� ����� ����� �� �����
        QuestPanel questPanel = Instantiate(_questPanelPrefab, _completedQuestsContent.transform).GetComponent<QuestPanel>();
        questPanel.InitializeAsFinished(savedPanel);
    }

    public QuestsSaveData SaveData()
    {
        QuestsSaveData saveData = new();
        foreach (QuestPanel panel in _activeQuests)
        {
            //�������� ������
            saveData.activeQuestTypes.Add(panel.QuestScriptName);
        }
        foreach (QuestPanel panel in _completedQuests)
        {
            if (panel.Quest == null)
            {
                //�����, ��������� ����������� �����.
                saveData.finishedQuestPanels.Add(new QuestsSaveData.SavedQuestPanel(panel));
            }
            else
            {
                //����������� �����, �� ������� �� ��������
                saveData.rewardUncollectedQuestTypes.Add(panel.QuestScriptName);
            }
        }
        return saveData;
    }

    public void LoadData(QuestsSaveData data)
    {
        foreach (string questName in data.activeQuestTypes)
        {
            QuestHandler.AddQuest(questName);
        }
        foreach (string questName in data.rewardUncollectedQuestTypes)
        {
            QuestHandler.AddQuestAsCompleted(questName);
            
        }
        foreach (QuestsSaveData.SavedQuestPanel questPanel in data.finishedQuestPanels)
        {
            AddQuestAsFinished(questPanel);
        }
    }
}
