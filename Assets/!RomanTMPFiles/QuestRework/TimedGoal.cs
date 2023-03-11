using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roman.Rework
{
    /// <summary>
    /// ����, ������� ����������� �� ���������� RequiredAmount �����
    ///�� ������ � WaitingGoal <��. cref ="WaitingGoal" />
    /// </summary>
    public class TimedGoal : Goal
    {
        private int _timeCounter;
        public TimedGoal(GoalParams goalParams): base(goalParams)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            Evaluate();

            GameTime.MinuteChanged += OnMinuteChanged;
        }
        public override void Deinitialize()
        {
            base.Deinitialize();

            GameTime.MinuteChanged -= OnMinuteChanged;
        }

        private void OnMinuteChanged()
        {
            _timeCounter++;
            if (_timeCounter >= 60)
            {
                _timeCounter = 0;
                CurrentAmount++;
                Evaluate();
            }
        }

        protected override void Evaluate()
        {
            UpdateGoal();

            if (CurrentState == State.Active)
            {
                if (CurrentAmount >= RequiredAmount)
                {
                    CurrentState = State.Failed;
                    Deinitialize(); //����� ��������� �������, ���� ����� �� ����� �����
                }
            }
        }

    }
}
