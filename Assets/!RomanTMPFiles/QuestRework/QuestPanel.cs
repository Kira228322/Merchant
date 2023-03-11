using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _questNameText;
    [SerializeField] private TMP_Text _shortSummaryText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private List<TMP_Text> _goalTexts = new();
    [SerializeField] private List<TMP_Text> _rewardTexts = new();

    [SerializeField] private Button _rewardButton;

    [SerializeField] private Color _activeGoalColor;
    [SerializeField] private Color _completedGoalColor;
    [SerializeField] private Color _failedGoalColor;

    [SerializeField] private Color _activeQuestNameColor;               //что-то вроде белого цвета
    [SerializeField] private Color _rewardUncollectedQuestNameColor;    //что-то вроде зеленого цвета
    [SerializeField] private Color _completedQuestNameColor;            //что-то вроде тёмно-серого цвета
    [SerializeField] private Color _failedQuestNameColor;               //что-то вроде тёмно-красного цвета

    private Quest _quest;

    public void Initialize(Quest quest)
    {
        _quest = quest;
        quest.questPanel = this;

        _questNameText.text = _quest.QuestName;
        _shortSummaryText.text = _quest.QuestSummary;
        _descriptionText.text = _quest.Description;

        for (int i = 0; i < _quest.Goals.Count; i++)
        {
            _goalTexts[i].gameObject.SetActive(true);
            _goalTexts[i].text = _quest.Goals[i].Description + " " + _quest.Goals[i].CurrentAmount + " / " + _quest.Goals[i].RequiredAmount;
        }

        if (_quest.ExperienceReward != 0)
        {
            _rewardTexts[0].gameObject.SetActive(true);
            _rewardTexts[0].text = _quest.ExperienceReward.ToString() + " опыта";
        }
        if (_quest.MoneyReward != 0)
        {
            _rewardTexts[1].gameObject.SetActive(true);
            _rewardTexts[1].text = _quest.MoneyReward.ToString() + " золота";
        }
        if (_quest.ItemRewards.Count != 0)
        {
            for (int i = 0; i < _quest.ItemRewards.Count; i++)
            {
                _rewardTexts[i + 2].gameObject.SetActive(true);
                _rewardTexts[i + 2].text = _quest.ItemRewards[i].itemName + ". Кол-во: " + _quest.ItemRewards[i].amount;
            }
        }

        Refresh();
    }
    public void Refresh()
    {
        switch (_quest.CurrentState)
        {
            case Quest.State.Active:
                _questNameText.color = _activeQuestNameColor;
                break;

            case Quest.State.RewardUncollected:
                _rewardButton.gameObject.SetActive(true);
                _questNameText.color = _rewardUncollectedQuestNameColor;
                break;

            case Quest.State.Completed:
                _questNameText.color = _completedQuestNameColor;
                break;

            case Quest.State.Failed:
                _questNameText.color = _failedQuestNameColor;
                break;

        }

        for (int i = 0; i < _quest.Goals.Count; i++)
        {
            _goalTexts[i].text = _quest.Goals[i].Description + ": " + _quest.Goals[i].CurrentAmount + " / " + _quest.Goals[i].RequiredAmount;
            switch (_quest.Goals[i].CurrentState)
            {
                case Goal.State.Active:
                    _goalTexts[i].color = _activeGoalColor;
                    break;

                case Goal.State.Completed:
                    _goalTexts[i].color = _completedGoalColor;
                    break;

                case Goal.State.Failed:
                    _goalTexts[i].color = _failedGoalColor;
                    break;
            }
        }
    }
    public void OnRewardButtonClick()
    {
        if (_quest.ItemRewards.Count != 0 && !InventoryController.Instance.IsThereAvailableSpaceForInsertingMultipleItems(Player.Instance.Inventory.ItemGrid, _quest.ItemRewards))
        {
            CanvasWarningGenerator.Instance.CreateWarning("Предупреждение", "В инвентаре недостаточно места!");
        }
        else
        {
            _quest.GiveReward();
            _rewardButton.gameObject.SetActive(false);
            Refresh();
        }
    }

}