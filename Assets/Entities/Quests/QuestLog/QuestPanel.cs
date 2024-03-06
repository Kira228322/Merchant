using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _questNameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private List<TMP_Text> _goalTexts = new();
    [SerializeField] private List<TMP_Text> _rewardTexts = new();

    [SerializeField] private Button _rewardButton;

    [SerializeField] private Sprite _completedGoalSprite;
    [SerializeField] private Sprite _uncompletedGoalSprite;

    [SerializeField] private Color _activeGoalColor;
    [SerializeField] private Color _completedGoalColor;
    [SerializeField] private Color _failedGoalColor;

    [SerializeField] private Color _activeQuestNameColor;               //что-то вроде белого цвета
    [SerializeField] private Color _rewardUncollectedQuestNameColor;    //что-то вроде зеленого цвета
    [SerializeField] private Color _completedQuestNameColor;            //что-то вроде тёмно-серого цвета
    [SerializeField] private Color _failedQuestNameColor;               //что-то вроде тёмно-красного цвета

    private Quest _quest;

    public Quest Quest => _quest;

    public void Initialize(Quest quest)
    {
        _quest = quest;
        quest.questPanel = this;

        _questNameText.text = _quest.QuestName;
        _descriptionText.text = _quest.Description;

        for (int i = 0; i < _quest.Goals.Count; i++)
        {
            _goalTexts[i].gameObject.SetActive(true);
            Image completionImage = _goalTexts[i].gameObject.GetComponentInChildren<Image>();
            if (_quest.Goals[i].RequiredAmount == 1)
            {
                completionImage.gameObject.SetActive(true);
                if (_quest.Goals[i].CurrentAmount == _quest.Goals[i].RequiredAmount)
                {
                    completionImage.sprite = _completedGoalSprite;
                }
                else
                {
                    completionImage.sprite = _uncompletedGoalSprite;
                }
                _goalTexts[i].text = _quest.Goals[i].Description + " ";
            }
            else
            {
                completionImage.gameObject.SetActive(false);
                if (_quest.Goals[i] is TimedGoal or WaitingGoal)
                {
                    _goalTexts[i].text = _quest.Goals[i].Description + ": " + GetFormattedTime(_quest.Goals[i].CurrentAmount) + " / " + GetFormattedTime(_quest.Goals[i].RequiredAmount);
                }
                else _goalTexts[i].text = _quest.Goals[i].Description + ": " + _quest.Goals[i].CurrentAmount + " / " + _quest.Goals[i].RequiredAmount;
            }
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

    private string GetFormattedTime(int totalhours)
    {
        int days = totalhours / 24;
        int hoursRemainder = totalhours % 24;
        if (days == 0)
            return totalhours + " " + TravelTimeCounter.GetLocalizedTime(totalhours, true);
        else
        {
            string result = days + " " + TravelTimeCounter.GetLocalizedTime(days, false) + " " + hoursRemainder + " " + TravelTimeCounter.GetLocalizedTime(hoursRemainder, true);
            return result;
        }
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
            Image mark = _goalTexts[i].GetComponentInChildren<Image>(true);
            if (_quest.Goals[i].RequiredAmount == 1)
                _goalTexts[i].text = _quest.Goals[i].Description + " ";
            else
            {
                if (_quest.Goals[i] is TimedGoal or WaitingGoal)
                {
                    _goalTexts[i].text = _quest.Goals[i].Description + ": " + GetFormattedTime(_quest.Goals[i].CurrentAmount) + " / " + GetFormattedTime(_quest.Goals[i].RequiredAmount);
                }
                else _goalTexts[i].text = _quest.Goals[i].Description + ": " + _quest.Goals[i].CurrentAmount + " / " + _quest.Goals[i].RequiredAmount;
            }
            switch (_quest.Goals[i].CurrentState)
            {
                case Goal.State.Active:
                    _goalTexts[i].color = _activeGoalColor;
                    mark.sprite = _uncompletedGoalSprite;
                    break;

                case Goal.State.Completed:
                    _goalTexts[i].color = _completedGoalColor;
                    mark.sprite = _completedGoalSprite;
                    break;

                case Goal.State.Failed:
                    _goalTexts[i].color = _failedGoalColor;
                    mark.sprite = _uncompletedGoalSprite;
                    break;
            }
        }
    }
    public void OnRewardButtonClick()
    {
        if (_quest.ItemRewards.Count != 0 && !InventoryController.Instance.CanInsertMultipleItems(Player.Instance.Inventory.BaseItemGrid, _quest.ItemRewards))
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