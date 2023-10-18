using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    #region ����, �������� � �������
    [SerializeField] private GameObject _blockerPanel;
    [SerializeField] private GameObject _dialogueWindow;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private GameObject[] _choices;
    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _lineFinishedIndicator;
    [SerializeField] private float _typingSpeed = 25f;

    [SerializeField] private ItemContainer _itemContainer;

    private TMP_Text[] _choicesText;
    private Npc _currentNPC;
    private Story _currentStory;
    private Coroutine _currentDisplayLineCoroutine;
    private bool _isCurrentlyWritingALine;

    private DeliveryGoal _currentDeliveryGoal;

    //������ �������� - � ��� ���������. ������ �������� - � ���.
    public event UnityAction<Npc, string> TalkedToNPCAboutSomething;
    #endregion

    #region External ������� Ink � �� ��� ������� � ����
    private void SetTextColor(string colorName)
    {
        ColorUtility.TryParseHtmlString(colorName, out Color result);
        //������ HtmlString? ������ ��� ����� ����� ��� ������� � ����������, � �� �������� ��� ����� ������
        _dialogueText.color = result;
    }
    private void OnDepositSuccessful()
    {
        _itemContainer.DepositAborted -= OnDepositAborted;
        _itemContainer.DepositSuccessful -= OnDepositSuccessful;
        _dialogueWindow.SetActive(true);
        if (_currentStory.currentChoices.Count != 0)
            DisplayChoices();
        _currentDeliveryGoal.CurrentAmount = _currentDeliveryGoal.RequiredAmount;
        _currentDeliveryGoal.Initialize(); //���� �������, �������� ������ Evaluate() ���� � Initiazize() ������ ���.
        _currentDeliveryGoal = null;
    }
    private void OnDepositAborted()
    {
        _itemContainer.DepositAborted -= OnDepositAborted;
        _itemContainer.DepositSuccessful -= OnDepositSuccessful;
        _currentDeliveryGoal = null;
        ExitDialogueMode();
    }
    private void BindFunctions()
    {
        //��� ������� ����� ����������� ������ ���, ����� ������������ ����� Story.
        //�� ����� ����, ��� �����, � � ������ ���������� ���� �� ����� ���������.
        //�� �� ����� ����, ��� ��������� ��� ������� �������� � ������ � �������� �� ��� ������
        //������� ������� ������� ������ ��� ��� ������ � ������

        _currentStory.BindExternalFunction("get_quest_state", (string questSummary) =>
        {
            Quest quest = QuestHandler.GetQuestBySummary(questSummary);
            if (quest == null)
            {
                return "null";
            }
            return quest.CurrentState.ToString();
        });
        _currentStory.BindExternalFunction("check_if_quest_has_been_taken", (string questSummary) =>
        {
            return QuestHandler.HasQuestBeenTaken(questSummary);
        });
        _currentStory.BindExternalFunction("set_color", (string colorName) =>
        {
            SetTextColor(colorName);
        });
        _currentStory.BindExternalFunction("add_quest", (string questSummary) =>
        {
            QuestHandler.AddQuest(PregenQuestDatabase.GetQuestParams(questSummary), _currentNPC.NpcData);
        });
        _currentStory.BindExternalFunction("invoke_dialogue_event", (string param) =>
        {
            TalkedToNPCAboutSomething?.Invoke(_currentNPC, param);
        });
        _currentStory.BindExternalFunction("debug_log", (string param) =>
        {
            print(param);
        });
        _currentStory.BindExternalFunction("has_enough_items", (string questSummary) =>
        {
            Quest quest = QuestHandler.GetActiveQuestBySummary(questSummary);
            if (quest != null)
            {
                foreach (Goal goal in quest.Goals)
                {
                    if (goal is GiveItemsGoal giveItemsGoal)
                    {
                        if (giveItemsGoal.RequiredIDOfNPC == _currentNPC.NpcData.ID)
                        {
                            if (!Player.Instance.Inventory.HasEnoughItemsOfThisItemData(
                                ItemDatabase.GetItem(giveItemsGoal.RequiredItemName),
                                giveItemsGoal.RequiredAmount))
                                return false;
                        }
                    }
                }
                return true;
            }
            return "null";
        });
        _currentStory.BindExternalFunction("is_questgiver_ready_to_give_quest", () =>
        {
            if (_currentNPC.NpcData is NpcQuestGiverData npcQuestGiverData)
            {
                return npcQuestGiverData.IsReadyToGiveQuest();
            }
            Debug.LogError("� Ink ��������������, ��� ���� Npc QuestGiver, � �� ����� ���� �� ���. ������ � ��������� �������");
            return "null";
        });
        _currentStory.BindExternalFunction("get_random_questparams_of_questgiver", () =>
        {
            if (_currentNPC.NpcData is NpcQuestGiverData npcQuestGiverData)
            {
                return npcQuestGiverData.GiveRandomQuest().questSummary;
            }
            Debug.LogError("� Ink ��������������, ��� ���� Npc QuestGiver, � �� ����� ���� �� ���. ������ � ��������� �������");
            return "null";
        });
        _currentStory.BindExternalFunction("get_activeQuestList", () =>
        {
            //UPD 03.08.23: ������ � ����, ��� ���������� �������,
            //� ������������� ��� � ��� �� ����� ���� ���� ������� ���������� �����.
            //������� ����� ���� ���� ���������
            //UPD 09.08.23: ����� ��������������� ������������ ����������,
            //��� ����� � �� ����, ��� ���������� �������. ���� �������� ������������� �������:
            //����� �� ���������� �������� ������, � ����� ������� ����.������� contains ����� ����������,
            //�������� �� ��� ������ ������.
            //�������� ������ zheba_strela � zheba_cactus ����������� ��� zheba_strelazheba_cactus
            List<Quest> activeQuests = QuestHandler.GetActiveQuestsForThisNPC(_currentNPC.NpcData.ID);
            string result = "";
            if (activeQuests.Count > 0)
            {
                foreach (Quest quest in activeQuests)
                {
                    result += quest.QuestSummary;
                }
            }
            return result;

        });
        _currentStory.BindExternalFunction("contains", (string source, string substring) =>
        {
            if (source.Contains(substring))
                return true;
            return false;
        });
        _currentStory.BindExternalFunction("is_empty", (string str) =>
        {
            return str == "";
        });
        _currentStory.BindExternalFunction("open_item_container", (string questSummary, int deliveryGoalNumber) =>
        {
            Quest quest = QuestHandler.GetActiveQuestBySummary(questSummary);
            if (quest != null)
            {
                if (quest.Goals[deliveryGoalNumber] is DeliveryGoal deliveryGoal)
                {
                    _itemContainer.Init(new(deliveryGoal.RequiredItemCategories), deliveryGoal.QuestItemsBehaviour, deliveryGoal.RequiredWeight, deliveryGoal.RequiredCount, deliveryGoal.RequiredRotThreshold, _currentNPC.NpcData.Name);
                    //open item container, halt dialogue;
                    _itemContainer.ShowItselfAndInventory(true);
                    HideChoices();
                    _dialogueWindow.SetActive(false);
                    _currentDeliveryGoal = deliveryGoal;
                    _itemContainer.DepositAborted += OnDepositAborted;
                    _itemContainer.DepositSuccessful += OnDepositSuccessful;
                }
                else
                {
                    Debug.LogError("� ����� ������ ��� DeliveryGoal ��� ���� ��������, ������ � ��������� �������");
                }
            }
            else
            {
                Debug.LogError("������ ������ ��� � ��������, ������ ��� ��������� �������");
            }
        });
        _currentStory.BindExternalFunction("has_money", (string amount) =>
        {
            return Player.Instance.Money >= int.Parse(amount);
        });
        _currentStory.BindExternalFunction("change_player_money", (string amount) =>
        {
            Player.Instance.Money += int.Parse(amount);
        });
        _currentStory.BindExternalFunction("place_item", (string itemName, string amount, string daysBoughtAgo) =>
        {
            InventoryController.Instance.TryCreateAndInsertItem(Player.Instance.ItemGrid, ItemDatabase.GetItem(itemName), int.Parse(amount), float.Parse(daysBoughtAgo), true, true);
        });
    }
    #endregion

    #region ������ �������������
    private void InitializeErrorHandler()
    {
        _currentStory.onError += (message, type) =>
        {
            if (type == Ink.ErrorType.Warning)
                Debug.LogWarning(message);
            else
                Debug.LogError(message);
        };
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("��� ���������� ������ ��������");
        }

        Instance = this;

        _choicesText = new TMP_Text[_choices.Length];
        for (int i = 0; i < _choices.Length; i++)
        {
            _choicesText[i] = _choices[i].GetComponentInChildren<TMP_Text>(true);
            _choices[i].SetActive(false);
        }

        _dialogueWindow.SetActive(false);
        _dialogueText.text = "";
    }
    #endregion

    #region ���������� ������ ������ � �������� (������ �������� �����, ��������/�������� ������)
    private IEnumerator DisplayLine(string line)
    {
        _isCurrentlyWritingALine = true;
        _lineFinishedIndicator.SetActive(false);
        _dialogueText.text = line;
        _dialogueText.enableAutoSizing = true;  //������������, ��� ������ ��� ��������� �������� � �������� ��������,
        _dialogueText.ForceMeshUpdate();        //����� ��������� ������ ��� ��������� ���������� ������. 
        _dialogueText.enableAutoSizing = false; //����� ��������� �������� � ���������� ������, ����� ����� �������� �����������.   
        _dialogueText.text = "";

        HideChoices();

        WaitForSeconds waitForSeconds = new(1 / _typingSpeed);
        foreach (char letter in line.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return waitForSeconds;
        }

        _isCurrentlyWritingALine = false;
        _lineFinishedIndicator.SetActive(true);
        if (_currentStory.currentChoices.Count != 0)
            DisplayChoices();
    }
    private void HideChoices()
    {
        foreach (GameObject choiceButton in _choices)
        {
            choiceButton.SetActive(false);
        }
    }
    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        _continueButton.interactable = false;
        for (int i = 0; i < currentChoices.Count; i++)
        {
            _choices[i].SetActive(true);
            _choicesText[i].text = currentChoices[i].text;
        }
    }
    #endregion

    #region ������� ������ ������ � �������� (���������� �������, ������ � ��������� ������)
    public void EnterDialogueMode(Npc npc)
    {
        Player.Instance.PlayerMover.DisableMove();
        _blockerPanel.SetActive(true);
        _currentNPC = npc;
        TextAsset npcInkJson = _currentNPC.InkJSON;
        _currentStory = new Story(npcInkJson.text);
        InitializeErrorHandler();
        BindFunctions();
        _dialogueWindow.SetActive(true);
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            string newLine = _currentStory.Continue();
            _currentDisplayLineCoroutine = StartCoroutine(DisplayLine(newLine));
        }
        else if (_currentStory.currentChoices.Count == 0) 
            ExitDialogueMode();
    }
    
    private void ExitDialogueMode()
    {
        Player.Instance.PlayerMover.EnableMove();
        _blockerPanel.SetActive(false);
        _dialogueWindow.SetActive(false);
        _dialogueText.text = "";
        _currentNPC.StopInteraction();
    }
    #endregion

    #region ����� ������
    public void OnContinueButtonClick()
    {
        if (_isCurrentlyWritingALine)
        {
            StopCoroutine(_currentDisplayLineCoroutine);
            _currentDisplayLineCoroutine = null;
            _isCurrentlyWritingALine = false;

            _dialogueText.text = _currentStory.currentText;

            if (_currentStory.currentChoices.Count != 0)
                DisplayChoices();

            _lineFinishedIndicator.SetActive(true);
        }
        else ContinueStory();
    }
    public void MakeChoice(int index)
    {
        _continueButton.interactable = true;
        _currentStory.ChooseChoiceIndex(index);
        foreach (var choice in _choices)
        {
            choice.SetActive(false);
        }
        ContinueStory();
    }
    #endregion

}
