using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roman.Rework
{
    /// <summary>
    /// Цель, которая выполняется по прошествию RequiredAmount часов
    ///Не путать с TimedGoal <см. cref ="TimedGoal" />
    /// </summary>
    public class WaitingGoal : Goal
    {
        private int _timeCounter;
        public WaitingGoal(GoalParams goalParams) : base(goalParams)
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
                    CurrentState = State.Completed;
                    Deinitialize(); //Можно перестать считать, ведь время не уйдет назад
                }
            }
        }

    }
}
