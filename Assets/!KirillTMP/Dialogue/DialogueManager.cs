using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private GameObject[] _choices;
    [SerializeField] private GameObject _continueButton;
    private TMP_Text[] _choicesText;
    private Story _currentStory;
    public static DialogueManager Instance;
    public event UnityAction<NPC> DialogStartedWithNPC;
    
    
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
        TextAsset npcInkJson = npc.InkJSON;
        _currentStory = new Story(npcInkJson.text);
        _currentStory.variablesState["affinity"] = npc.Affinity;
        _dialoguePanel.SetActive(true);
        DialogStartedWithNPC?.Invoke(npc);
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            _dialogueText.text = _currentStory.Continue();
            if (_currentStory.currentChoices.Count != 0)
                DisplayChoices();
        }
        else ExitDialogueMode();
    }

    private void ExitDialogueMode()
    {
        _dialoguePanel.SetActive(false);
        _dialogueText.text = "";
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        _continueButton.SetActive(false);
        for (int i = 0; i < currentChoices.Count; i++)
        {
            _choices[i].SetActive(true);
            _choicesText[i].text = currentChoices[i].text;
        }
    }

    public void MakeChoice(int index)
    {
        _continueButton.SetActive(true);
        _currentStory.ChooseChoiceIndex(index);
        foreach (var choice in _choices)
        {
            choice.SetActive(false);
        }
        ContinueStory();
    }
}
