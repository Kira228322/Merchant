using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialStateTracker : MonoBehaviour, ISaveable<TutorialStateTrackerSaveData>
{
    [Serializable]
    public class PresentationInfo
    {
        public string RequiredQuestSummary;
        public string PresentationSummary;
        //��������, ������� �������� ��� ���� ��� ����� ��� ��������� -
        //����� ����� summary ������� ������ � summary ������ �����������.
        //�� ������ ��� ����� ���������, ������ ��� ������ TutorialPresentation �������� SO,
        //� ������, �� �������������. �� � ����� ��� ��������� ��� ������, no problemo

    }

    [SerializeField] private List<PresentationInfo> _tutorialPresentations;

    private Dictionary<PresentationInfo, bool> QuestTriggerConditions = new();

    private void OnEnable()
    {
        QuestHandler.QuestChangedState += OnQuestChangedState;
    }
    private void OnDisable()
    {
        QuestHandler.QuestChangedState -= OnQuestChangedState;
    }


    private void Start()
    {
        InitialDictionarySet(_tutorialPresentations);
    }

    private void InitialDictionarySet(List<PresentationInfo> infos)
    {
        Dictionary<PresentationInfo, bool> newDictionary = new();
        foreach (PresentationInfo info in infos)
        {
            newDictionary.Add(info, false);
        }
        QuestTriggerConditions = newDictionary;
    }

    private void OnQuestChangedState(Quest quest)
    {
        if (quest.CurrentState != Quest.State.Active)
            return;

        PresentationInfo foundPresentation = QuestTriggerConditions.FirstOrDefault
            (presentation => presentation.Key.RequiredQuestSummary == quest.QuestSummary).Key;

        if (foundPresentation == null)
            return;
        if (!QuestTriggerConditions[foundPresentation]) //���� ����������� ��� �� ���� �������� ������
        {
            PresentationDisplayer.Instance.ShowPresentation(foundPresentation.PresentationSummary);
            QuestTriggerConditions[foundPresentation] = true; //������ ��� ����� �������� (�.� true),
                                                              //������ �� ����� ��������

            Diary.Instance.AddTutorial(foundPresentation);
        }
    }

    public void AddAllPresentations()
    {
        foreach (PresentationInfo info in QuestTriggerConditions.Keys)
        {
            Diary.Instance.AddTutorial(info);
        }
    }

    public void LoadData(TutorialStateTrackerSaveData data)
    {
        QuestTriggerConditions = data.SavedDictionary;

        Diary diary = Diary.Instance;
        foreach (var savedPresentation in QuestTriggerConditions)
        {
            if (savedPresentation.Value) //���� ��� ���� �������� ������
            {
                diary.AddTutorial(savedPresentation.Key);
            }
        }

        if (QuestTriggerConditions.All(item => item.Value))
        {
            QuestHandler.QuestChangedState -= OnQuestChangedState;
        }
    }

    public TutorialStateTrackerSaveData SaveData()
    {
        ;
        TutorialStateTrackerSaveData saveData = new(QuestTriggerConditions);
        return saveData;
    }
}
