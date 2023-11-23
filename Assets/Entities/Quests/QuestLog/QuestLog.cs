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
        //TODO: сортировка по дате вз€ти€ после добавлени€ новых квестов

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
        else // весты без цепочки могут отправл€тьс€ в цепочку "–азное". Ёто фейк-цепочка
        {
            if (_activeQuestLines[null] == null) //[null] будет соответствовать "–азное"
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
        else // весты без цепочки могут отправл€тьс€ в цепочку "–азное". Ёто фейк-цепочка
        {
            if (_activeQuestLines[null] == null) //[null] будет соответствовать "–азное"
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
        //¬ целом так и остаЄтс€, нужно удалить квест внутри его цепочки и заспавнить его в цепочке другого раздела
        Destroy(quest.questPanel.gameObject);
        CheckQuestLineAndAddNewPanel(quest, newState);
  
    }
    private void CheckQuestLineAndAddNewPanel(Quest quest, Quest.State newState)
    {
        //ƒумаю: получать через Quest quest.QuestSummary PregenQuestSO через базу данных. ” него мы знаем его QuestLine, знаем первый он или последний.
        //≈сли последний и он выполн€етс€, то играть анимацию завершени€ цепочки.
        //≈сли первый и он добавл€етс€, то создавать цепочку.
        //≈сли добавл€етс€ не первый, добавл€ть его в существующую цепочку. (“ут надо внимательно насчЄт сохранени€ - если первый уже завершен?)
        //(“о есть если цепочки нет, создавать еЄ всЄ равно)
        
        //Ќа самом деле всегда, когда добавл€ем квест, нужно чекать, а есть ли уже така€ цепочка.
        //≈сли нету, создавать еЄ. ≈сли квест активен и он последний в цепочке, то еЄ нужно удалить.
        //≈сли мы говорим про завершенные цепочки, они не удал€ютс€ никогда.

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
            //≈сли квест последний в цепочке и дл€ него создана QuestLinePanel (а она не будет создана, если это загрузка) то удалить еЄ.
            // вест никогда не становитс€ активным из выполненного, а выполненные цепочки никогда не уничтожаютс€.
            if (isLast && _activeQuestLines[questLine] != null)
            {
                Destroy(_activeQuestLines[questLine]);
                _activeQuestLines.Remove(questLine);
            }
            AddToFinishedQuests(quest, questLine);
        }

        //TODO сортировать список панелек по дате вз€ти€/выполнени€ после добавлени€ квеста?
    }
    private void OnQuestUpdated(Quest quest)
    {
        quest.questPanel.Refresh();
    }

}