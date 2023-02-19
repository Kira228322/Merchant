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

    [SerializeField] private Button _rewardButton;

    [SerializeField] private Color _activeGoalColor;
    [SerializeField] private Color _completedGoalColor;
    [SerializeField] private Color _completedQuestNameColor;
    [SerializeField] private Color _completedAndRewardAcquiredQuestNameColor;

    public Quest Quest;

    public void Initialize(Quest quest)
    {
        Quest = quest;
        _questNameText.text = Quest.QuestName;
        if (quest.IsCompleted)
        {
            _rewardButton.gameObject.SetActive(true);
            _questNameText.color = _completedGoalColor;
        }
        _descriptionText.text = "Описание: " + Quest.Description;

        for (int i = 0; i < Quest.Goals.Count; i++)
        {
            _goalTexts[i].gameObject.SetActive(true);
            _goalTexts[i].text = Quest.Goals[i].Description + Quest.Goals[i].CurrentAmount + " / " + Quest.Goals[i].RequiredAmount;
        }

        _rewardTexts[0].text = Quest.ExperienceReward.ToString() + " опыта";
        _rewardTexts[1].text = Quest.MoneyReward.ToString() + " золота";
        if (Quest.ItemRewards.Count != 0)
        {
            int i = 2;
            foreach (var reward in Quest.ItemRewards)
            {
                _rewardTexts[i].gameObject.SetActive(true);
                _rewardTexts[i].text = reward.item.Name + ". Кол-во: " + reward.amount;
                i++;
            }
        }
        Quest.questPanel = this;
        Refresh();
    }
    public void Refresh()
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
    public void OnComplete()
    {
        _rewardButton.gameObject.SetActive(true);
        _questNameText.color = _completedQuestNameColor;
    }
    public void OnRewardButtonClick()
    {
        
        if (Quest.ItemRewards.Count != 0 && !InventoryController.Instance.IsThereAvailableSpaceForInsertingMultipleItems(Player.Instance.Inventory.ItemGrid, Quest.ItemRewards))
        {
            CanvasWarningGenerator.Instance.CreateWarning("Предупреждение", "В инвентаре недостаточно места!");
        }
        else
        {
            Quest.GiveReward();
            QuestHandler.RemoveQuest(Quest.GetType());
            _questNameText.color = _completedAndRewardAcquiredQuestNameColor;
            _rewardButton.gameObject.SetActive(false);
        }
    }
}
