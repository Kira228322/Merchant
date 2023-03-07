using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    #region ����, �������� � �������
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private GameObject[] _choices;
    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _lineFinishedIndicator;
    [Tooltip("��� ������ ��������, ��� ������� ���������� �����. 1/Typing Speed")]
    [SerializeField]private float _typingSpeed = 0.04f;

    private TMP_Text[] _choicesText;
    private NPC _currentNPC;
    private Story _currentStory;
    private Coroutine _currentDisplayLineCoroutine;
    private bool _isCurrentlyWritingALine;
    private List<string> _currentTags = new();

    //������ �������� - � ��� ���������. ������ �������� - � ���.
    public event UnityAction<NPC, string> TalkedToNPCAboutSomething;
    #endregion

    #region External ������� Ink � �� ��� ������� � ����
    private bool IsQuestCompleted(string questName)
    {
        return QuestHandler.IsQuestCompleted(questName);
    }
    private bool IsQuestActive(string questName)
    {
        return QuestHandler.IsQuestActive(questName);
    }
    private void SetTextColor(string colorName)
    {
        ColorUtility.TryParseHtmlString(colorName, out Color result);
        //������ HtmlString? ������ ��� ����� ����� ��� ������� � ����������, � �� �������� ��� ����� ������
        _dialogueText.color = result;
    }
    private void BindFunctions()
    {
        //��� ������� ����� ����������� ������ ���, ����� ������������ ����� Story.
        //�� ����� ����, ��� �����, � � ������ ���������� ���� �� ����� ���������.
        //�� �� ����� ����, ��� ��������� ��� ������� �������� � ������ � �������� �� ��� ������
        //������� ������� ������� ������ ��� ��� ������ � ������

        _currentStory.BindExternalFunction("check_if_quest_active", (string param) =>
        { return IsQuestActive(param); 
        });

        _currentStory.BindExternalFunction("check_if_quest_completed", (string param) =>
        {
           return IsQuestCompleted(param);
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
            _choicesText[i] = _choices[i].GetComponentInChildren<TMP_Text>();
            _choices[i].SetActive(false);
        }

        ExitDialogueMode();
    }
    #endregion

    #region ���������� ������ ������ � �������� (������ �������� �����, ��������� ����, ��������/�������� ������)
    private void ParseTags()
    {
        _currentTags = _currentStory.currentTags;
        foreach (string tag in _currentTags)
        {
            string prefix = tag.Split(' ')[0]; //������ ���������� �����: #give_quest TestQuestFindApples
            string param = tag.Split(' ')[1];  //��� #color blue, ��� #add_affinity -5

            switch (prefix.ToLower())
            {
                case "give_quest":
                    QuestHandler.AddQuest(param);
                    break;

                case "color":
                    SetTextColor(param);
                    break;

                case "add_affinity":
                    _currentNPC.Affinity += int.Parse(param);
                    break;

                case "invoke": //������������ ����������� ������� ��� TalkToNPCGoal. Goal ������ ����� string param 
                    TalkedToNPCAboutSomething?.Invoke(_currentNPC, param);
                    break;
            }
        }
    }
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

        WaitForSeconds waitForSeconds = new(_typingSpeed);
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
    public void EnterDialogueMode(NPC npc)
    {
        //TODO ��������� ������ �������� ������, ����. ���������, playerMover, �����
        _currentNPC = npc;
        TextAsset npcInkJson = _currentNPC.InkJSON;
        _currentStory = new Story(npcInkJson.text);
        BindFunctions();
        InitializeErrorHandler();
        _currentStory.variablesState["affinity"] = _currentNPC.Affinity;
        _dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            string newLine = _currentStory.Continue();
            ParseTags();
            _currentDisplayLineCoroutine = StartCoroutine(DisplayLine(newLine));
        }
        else if (_currentStory.currentChoices.Count == 0) 
            ExitDialogueMode();
    }
    
    private void ExitDialogueMode()
    {
        _dialoguePanel.SetActive(false);
        _dialogueText.text = "";
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
