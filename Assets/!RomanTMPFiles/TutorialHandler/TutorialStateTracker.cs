using System;
using System.Collections;
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
        //—огласен, сделано неудобно дл€ того кто будет это вписывать -
        //нужно знать summary нужного квеста и summary нужной презентации.
        //Ќо только так можно сохран€ть, потому что просто TutorialPresentation €вл€етс€ SO,
        //а значит, не сериализуетс€. Ќу € готов сам составить эти списки, no problemo

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
        if (!QuestTriggerConditions[foundPresentation]) //если презентаци€ ещЄ не была показана игроку
        {
            PresentationDisplayer.Instance.ShowPresentation(foundPresentation.PresentationSummary);
            QuestTriggerConditions[foundPresentation] = true; //теперь она стала показана (т.е true),
                                                              //больше не будет показана
            
            //оставил на завтра
            //Diary.Instance.AddPresentation()
        }
    }

    public void LoadData(TutorialStateTrackerSaveData data)
    {
        QuestTriggerConditions = data.SavedDictionary;

        //TODO: ƒобавл€ть просмотренные презентации в Diary

        //TODO: ”ничтожать себ€ нельз€, потому что этот объект нужен GameManager дл€ сохранени€
        //(ќн будет вызывать FindObjectOfType, случитс€ Nullref). ≈сли все презентации просмотрены,
        //можно просто отписыватьс€ от ивента QuestHandler
    }

    public TutorialStateTrackerSaveData SaveData()
    {;
        TutorialStateTrackerSaveData saveData = new(QuestTriggerConditions);
        return saveData;
    }
}
