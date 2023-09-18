using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAppearanceController : MonoBehaviour
{
    [Serializable]
    private class AppearanceControls
    {
        public string QuestSummary;
        public Quest.State questState;
        
        public bool IsReady()
        {
            Quest quest = QuestHandler.GetQuestBySummary(QuestSummary);
            if (quest == null)
                return false;
            if (questState == quest.CurrentState)
                return true;
            if (questState == Quest.State.Completed && quest.CurrentState == Quest.State.RewardUncollected)
                return true;
            return false;
        }
    }
    private enum Appearance { Show, Hide }
    [SerializeField] private Appearance _appearance;

    [SerializeField] private List<AppearanceControls> _controls = new();

    private void Start()
    {
        CheckAppearance();
    }

    private void CheckAppearance()
    {
        bool shouldShow = _appearance == Appearance.Show;
        foreach (var control in _controls)
        {
            if (!control.IsReady())
            {
                shouldShow = !shouldShow;
                break;
            }
        }

        gameObject.SetActive(shouldShow);
    }
}
