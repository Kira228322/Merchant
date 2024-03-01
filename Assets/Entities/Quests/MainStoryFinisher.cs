using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStoryFinisher : MonoBehaviour
{
    // Скрипт который перекидывает на другую сцену после активации устройства по мейн сюжету

    private void OnEnable()
    {
        QuestHandler.QuestChangedState += OnQuestChangedState;
    }

    private void OnDisable()
    {
        QuestHandler.QuestChangedState += OnQuestChangedState;
    }

    private void OnQuestChangedState(Quest quest)
    {
        if (quest.QuestSummary == "activate_device" && (quest.CurrentState == Quest.State.Completed || quest.CurrentState == Quest.State.RewardUncollected))
        {
            FindObjectOfType<SceneTransiter>().StartTransit(MapManager.GetLocationBySceneName("Scene20"));
        }
    }

}
