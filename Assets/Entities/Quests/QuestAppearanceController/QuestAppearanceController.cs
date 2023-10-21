using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
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

    private enum AppearanceBehavior { HideThenShow, ShowThenHide}

    [SerializeField] private AppearanceBehavior _appearanceBehaviour;

    [SerializeField] private List<AppearanceControls> _showConditions = new();
    [SerializeField] private List<AppearanceControls> _hideConditions = new();

    //������� �� ��������� _appearanceBehaviour. ���� HideThenShow, �� ������ �������� �������.
    //���� ����������� ��� showConditions, �� ����������. ���� ����� ����������� hideConditions, �� ����� ����������.

    //���� ShowThenHide, �� �������� ����������.
    //���� ����������� HideConditions, �� ����������. ���� ����� ����������� ShowConditions, �� ����� ����������.

    private void Awake()
    {
        CheckAppearance();
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

    private bool Test(bool appearanceBehaviour, bool showControls, bool hideControls)
    {
        return (appearanceBehaviour && (showControls || !hideControls)) || (showControls && !hideControls);
    }
}
