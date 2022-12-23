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

    public Quest Quest;

    public void Initialize(Quest quest)
    {
        Quest = quest;
        _questNameText.text = Quest.QuestName;
        _descriptionText.text = "Описание: " + Quest.Description;

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
                _goalTexts[i].text = Quest.Goals[i].Description + ": " + Quest.Goals[i].CurrentAmount + " / " + Quest.Goals[i].RequiredAmount;
            }
        }
        
    }
    private void OnComplete()
    {
        Quest.QuestUpdatedEvent -= Refresh;
        Quest.QuestCompletedEvent -= OnComplete;
        _questNameText.color = Color.green; //пока так, потом можно будет как-то иначе показывать что сделано
    }
}
