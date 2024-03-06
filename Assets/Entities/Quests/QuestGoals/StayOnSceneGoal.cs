using System;

[Serializable]
public class StayOnSceneGoal : Goal
{
    //Goal, который фейлитс€, как только игрок начинает поездку на другую сцену

    [NonSerialized] public Location RequiredLocation;
    public string RequiredSceneName;

    public StayOnSceneGoal(State currentState, string description, int currentAmount, int requiredAmount, string requiredSceneName) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredSceneName = requiredSceneName;
    }

    public override void Initialize()
    {
        RequiredLocation = MapManager.GetLocationBySceneName(RequiredSceneName);

        MapManager.PlayerStartedTravel += OnPlayerStartedTravel;

        Evaluate();
    }

    public override void Deinitialize()
    {
        MapManager.PlayerStartedTravel -= OnPlayerStartedTravel;
    }

    private void OnPlayerStartedTravel(Location source, Location destination)
    {
        if (source == RequiredLocation)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
    protected override void Evaluate()
    {
        if (CurrentState == State.Completed)
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Failed;
                Deinitialize(); //ћожно перестать считать, ведь ошибок прошлого уже не исправить
            }
        }
        UpdateGoal();
    }
}
