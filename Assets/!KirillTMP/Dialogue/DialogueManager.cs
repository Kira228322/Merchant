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
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private GameObject[] _choices;
    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _lineFinishedIndicator;
    [SerializeField] private float _typingSpeed = 0.04f;
    private TMP_Text[] _choicesText;
    private Story _currentStory;
    public static DialogueManager Instance;
    public event UnityAction<NPC> DialogStartedWithNPC;

    private Coroutine _currentDisplayLineCoroutine;
    private bool _isCurrentlyWritingALine;

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

    public void EnterDialogueMode(NPC npc)
    {
        //Выключать другие действия игрока, напр. инвентарь, playerMover, карту
        TextAsset npcInkJson = npc.InkJSON;
        _currentStory = new Story(npcInkJson.text);
        _currentStory.variablesState["affinity"] = npc.Affinity;
        _dialoguePanel.SetActive(true);
        DialogStartedWithNPC?.Invoke(npc);
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (_isCurrentlyWritingALine)
        {
            StopCoroutine(_currentDisplayLineCoroutine);
            _currentDisplayLineCoroutine = null;
            _isCurrentlyWritingALine = false;
            if (_currentStory.currentChoices.Count != 0)
                DisplayChoices();

            _dialogueText.text = _currentStory.currentText;
            _lineFinishedIndicator.SetActive(true);
            return;
        }
        if (_currentStory.canContinue)
        {
            if (_currentDisplayLineCoroutine != null)
                StopCoroutine(_currentDisplayLineCoroutine);
            _currentDisplayLineCoroutine = StartCoroutine(DisplayLine(_currentStory.Continue()));
        }
        else if (_currentStory.currentChoices.Count == 0) 
            ExitDialogueMode();
    }
    private IEnumerator DisplayLine(string line)
    {
        _isCurrentlyWritingALine = true;
        _lineFinishedIndicator.SetActive(false);
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
    private void ExitDialogueMode()
    {
        _dialoguePanel.SetActive(false);
        _dialogueText.text = "";
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
}
