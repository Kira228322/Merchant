using System;
using UnityEngine;
using UnityEngine.Events;

//ѕочему не сделал Goal абстрактным? ѕотому что он должен сериализироватьс€, а типы, наследуемые от абстрактных, сериализировать нельз€.
[Serializable]
public class Goal
{
    [HideInInspector] public int IntendedIndex; //¬ результате внедрени€ новой системы сохранени€ при переходе на облако
                                                //яндекс »гр, пришлось сделать несколько списков дл€ каждого типа Goal и
                                                //при загрузке делать AddRange. Ёто привело к тому, что оригинальный индекс
                                                //Goal тер€лс€. Ёто свойство призвано запомнить оригинальный индекс.
    public enum State { Active, Completed, Failed }

    [SerializeField] private State _currentState;

    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            GoalUpdated?.Invoke(this);
        }
    }
    public string Description;
    public int CurrentAmount;
    public int RequiredAmount;


    public event UnityAction<Goal> GoalUpdated;

    public Goal(State currentState, string description, int currentAmount, int requiredAmount)
    {
        CurrentState = currentState;
        Description = description;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;
    }

    public Goal()
    {

    }

    protected void UpdateGoal()
    {
        GoalUpdated?.Invoke(this);
    }

    protected virtual void Evaluate()
    {
        if (CurrentState == State.Active)
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Completed;
            }
        }
        else if (CurrentState == State.Completed)  //–азвыполнить. ѕример: Ѕыло три €блока из трЄх => ¬ыполнилс€.
                                                   //„ел выкинул одно => –азвыполнилс€.
        {
            if (CurrentAmount < RequiredAmount)
            {
                CurrentState = State.Active;
            }
        }
    }

    public void Fail()
    {
        CurrentState = State.Failed;
    }

    public virtual void Initialize() { }
    public virtual void Deinitialize() { }

}
