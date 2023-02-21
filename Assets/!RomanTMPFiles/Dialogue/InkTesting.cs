using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkTesting : MonoBehaviour
{
    [SerializeField] private TextAsset _inkJSON;
    private Story _story;

    private void Start()
    {
        _story = new(_inkJSON.text);
        Debug.Log(LoadStoryChunk());

        foreach (Choice choice in _story.currentChoices)
        {
            Debug.Log(choice.text);
        }

        _story.ChooseChoiceIndex(0);
        Debug.Log(LoadStoryChunk());
    }

    private string LoadStoryChunk()
    {
        if (_story.canContinue)
        {
            return _story.ContinueMaximally();
        }
        return null;
    }
}
