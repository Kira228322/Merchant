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

    #region Поля, свойства и события
    [SerializeField] private GameObject _dialogueWindow;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private GameObject[] _choices;
    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _lineFinishedIndicator;
    [Tooltip("Чем меньше значение, тем быстрее печатается текст. 1/Typing Speed")]
    [SerializeField]private float _typingSpeed = 0.04f;

    private TMP_Text[] _choicesText;
    private NPC _currentNPC;
    private Story _currentStory;
    private Coroutine _currentDisplayLineCoroutine;
    private bool _isCurrentlyWritingALine;

    //Первый параметр - с кем поговорил. Второй параметр - о чём.
    public event UnityAction<NPC, string> TalkedToNPCAboutSomething;
    #endregion

    #region External функции Ink и всё что связано с ними
    private void SetTextColor(string colorName)
    {
        ColorUtility.TryParseHtmlString(colorName, out Color result);
        //Почему HtmlString? Потому что такой метод уже встроен в библиотеку, и он работает для этого случая
        _dialogueText.color = result;
    }
    private void BindFunctions()
    {
        //Эта функция будет срабатывать каждый раз, когда генерируется новая Story.
        //На самом деле, это глупо, и я сделал глобальный файл со всеми функциями.
        //Но не нашел пути, как забиндить эти функции отдельно в начале и возможно ли это вообще
        //Поэтому придётся биндить каждый раз при заходе в диалог

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
            QuestHandler.AddQuest(PregenQuestDatabase.GetQuestParams(questSummary));
        });
        _currentStory.BindExternalFunction("get_affinity_here", () =>
        {
            return _currentNPC.NpcData.Affinity;
        });
        _currentStory.BindExternalFunction("get_affinity_by_name", (string npcName) =>
        {
            return NPCDatabase.GetNPCData(npcName).Affinity;
        });
        _currentStory.BindExternalFunction("add_affinity", (string npcName, string amount) =>
        {
            NPCDatabase.GetNPCData(npcName).Affinity += int.Parse(amount);
        });
        _currentStory.BindExternalFunction("add_affinity_here", (string amount) =>
        {
            //Очень жаль, но Ink не поддерживает перегрузку external функции, так бы просто сделал add_affinity(string amount)
            _currentNPC.NpcData.Affinity += int.Parse(amount);
            Debug.Log(_currentNPC.NpcData.Affinity);
        });
        _currentStory.BindExternalFunction("invoke_dialogue_event", (string param) =>
        {
            TalkedToNPCAboutSomething?.Invoke(_currentNPC, param);
        });
        _currentStory.BindExternalFunction("debug_log", (string param) =>
        {
            Debug.Log(param);
        });

    }
    #endregion

    #region Методы инициализации
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
            Debug.Log("Уже существует диалог менеджер");
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

    #region Внутренние методы работы с диалогом (начать печатать текст, показать/спрятать ответы)
    private IEnumerator DisplayLine(string line)
    {
        _isCurrentlyWritingALine = true;
        _lineFinishedIndicator.SetActive(false);
        _dialogueText.text = line;
        _dialogueText.enableAutoSizing = true;  //Представляем, что строка уже полностью написана и включаем автоСайз,
        _dialogueText.ForceMeshUpdate();        //чтобы запомнить размер при полностью написанной строке. 
        _dialogueText.enableAutoSizing = false; //Затем Выключаем автосайз и опустошаем строку, чтобы снова написать посимвольно.   
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

    #region Внешние методы работы с диалогом (продвинуть историю, начать и закончить диалог)
    public void EnterDialogueMode(NPC npc)
    {
        //TODO Выключать другие действия игрока, напр. инвентарь, playerMover, карту
        _currentNPC = npc;
        TextAsset npcInkJson = _currentNPC.NpcData.InkJSON;
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
        _dialogueWindow.SetActive(false);
        _dialogueText.text = "";
    }
    #endregion

    #region Инпут игрока
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
