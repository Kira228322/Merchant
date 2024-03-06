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
        [Tooltip("������� ����� ������ ������ � ������� ���������� ������, ����� ��������� �������� � ����")]
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

    [Tooltip("������� �� ��������� _appearanceBehaviour. " +
        "���� HideThenShow, �� ������ �������� �������. " +
        "���� ����������� ��� showConditions, �� ����������. " +
        "���� ����� ����������� hideConditions, �� ����� ����������." +
        "\n" +
        "���� ShowThenHide, �� �������� ����������." +
        "���� ����������� HideConditions, �� ����������. " +
        "���� ����� ����������� ShowConditions, �� ����� ����������.)")]
    [SerializeField] private AppearanceBehavior _appearanceBehaviour;

    [Tooltip("���� true, ������ ��������/���������� ����� ����� ���������� ������, " +
        "� �� ��� ���������� �� �����.")]
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
        //������ ���������������� ������� � �������� 00101011,
        //��� x1 == appearanceBehaviour, x2 == showControls, x3 == hideControls

        gameObject.SetActive(result);
    }
}
