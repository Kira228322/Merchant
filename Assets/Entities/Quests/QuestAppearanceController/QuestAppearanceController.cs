using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestAppearanceController : MonoBehaviour
{
    [Serializable]
    private class AppearanceControls
    {
        public string QuestSummary;
        public Quest.State questState;
        [Tooltip("Сколько часов должно пройти с момента выполнения квеста, чтобы изменения вступили в силу")]
        public int HoursDelay;
        public bool IsReady()
        {
            Quest quest = QuestHandler.GetQuestBySummary(QuestSummary);

            if (quest != null && IsTimeExpired(quest.DayFinishedOn, quest.HourFinishedOn))
            {
                return (questState == quest.CurrentState) ||
                       (questState == Quest.State.Completed && quest.CurrentState == Quest.State.RewardUncollected);
            }

            return false;
        }

        private bool IsTimeExpired(int dayFinishedOn, int hourFinishedOn)
        {
            int totalHoursPassed = (GameTime.CurrentDay - dayFinishedOn) * 24 + GameTime.Hours - hourFinishedOn;

            return totalHoursPassed >= HoursDelay;
        }
    }

    private enum AppearanceBehavior { HideThenShow, ShowThenHide }

    [Tooltip("Смотрит на поведение _appearanceBehaviour. " +
        "Если HideThenShow, то объект начинает скрытым. " +
        "Если соблюдаются все showConditions, то появляется. " +
        "Если также соблюдаются hideConditions, то снова скрывается." +
        "\n" +
        "Если ShowThenHide, то начинает показанным." +
        "Если соблюдаются HideConditions, то скрываются. " +
        "Если также соблюдаются ShowConditions, то снова появляется.)")]
    [SerializeField] private AppearanceBehavior _appearanceBehaviour;

    [Tooltip("Если true, объект исчезает/появляется сразу после выполнения квеста, " +
        "а не при перезаходе на сцену.")]
    [SerializeField] private bool _isInstant;

    [SerializeField] private List<AppearanceControls> _showConditions = new();
    [SerializeField] private List<AppearanceControls> _hideConditions = new();



    private void Awake()
    {
        CheckAppearance();
        if (_isInstant)
            QuestHandler.QuestChangedState += InstantCheckAppearance;
    }
    private void OnDestroy()
    {
        if (_isInstant)
            QuestHandler.QuestChangedState -= InstantCheckAppearance;
    }

    private bool AreControlsReady(List<AppearanceControls> controls)
    {
        if (controls.Count == 0)
            return false;

        foreach (AppearanceControls control in controls)
        {
            if (!control.IsReady())
            {
                return false;
            }
        }
        return true;
    }

    private void InstantCheckAppearance(Quest quest)
    {
        if (_hideConditions.Any(control => control.QuestSummary == quest.QuestSummary)
            || _showConditions.Any(control => control.QuestSummary == quest.QuestSummary))
            CheckAppearance();
    }

    private void CheckAppearance()
    {
        bool showControls = AreControlsReady(_showConditions);
        bool hideControls = AreControlsReady(_hideConditions);
        bool appearanceBehaviour = _appearanceBehaviour == AppearanceBehavior.ShowThenHide;

        bool result = (appearanceBehaviour && (showControls || !hideControls)) || (showControls && !hideControls);
        //Просто минимизированная функция с вектором 00101011,
        //где x1 == appearanceBehaviour, x2 == showControls, x3 == hideControls

        gameObject.SetActive(result);
    }
}
