using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _questNameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private List<TMP_Text> _goalTexts = new();
    [SerializeField] private List<TMP_Text> _rewardTexts = new();

    [SerializeField] private Color _activeGoalColor;
    [SerializeField] private Color _completedGoalColor;
    [SerializeField] private Color _completedQuestNameColor;

    public Quest Quest;

    public void Initialize(Quest quest)
    {
        Quest = quest;
        _questNameText.text = Quest.QuestName;
        if (quest.IsCompleted) _questNameText.color = _completedGoalColor;
        _descriptionText.text = "ќписание: " + Quest.Description;

        for (int i = 0; i < Quest.Goals.Count; i++)
        {
            _goalTexts[i].gameObject.SetActive(true);
            _goalTexts[i].text = Quest.Goals[i].Description + Quest.Goals[i].CurrentAmount + " / " + Quest.Goals[i].RequiredAmount;
        }

        _rewardTexts[0].text = Quest.ExperienceReward.ToString() + " опыта";
        _rewardTexts[1].text = Quest.MoneyReward.ToString() + " золота";

        Quest.QuestUpdatedEvent += Refresh;
        Quest.QuestCompletedEvent += OnComplete;

        Refresh();
    }
    private void Refresh()
    {
        if (Quest != null)
        {
            for (int i = 0; i < Quest.Goals.Count; i++)
            {
                if (Quest.Goals[i].CurrentAmount < Quest.Goals[i].RequiredAmount)
                {
                    _goalTexts[i].text = Quest.Goals[i].Description + ": " + Quest.Goals[i].CurrentAmount + " / " + Quest.Goals[i].RequiredAmount;
                    _goalTexts[i].color = _activeGoalColor;
                }
                else 
                {
                    _goalTexts[i].text = Quest.Goals[i].Description + ": " + Quest.Goals[i].RequiredAmount + " / " + Quest.Goals[i].RequiredAmount;
                    _goalTexts[i].color = _completedGoalColor;
                }
            }
        }
        
    }
    private void OnComplete(Quest quest)
    {
        //Ёто не выполнитс€, если квест был сделан в тот же фрейм, как только вз€т.
        //ќднако на этот случай смена цвета учтена в инициализации, а отписки от ивентов должны происходить и так,
                                                                   //поскольку экземпл€р квеста скоро уничтожитс€.
        Quest.QuestUpdatedEvent -= Refresh;
        Quest.QuestCompletedEvent -= OnComplete;
        _questNameText.color = _completedQuestNameColor;
    }
}
