using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GlobalEvent_DangerousRoad : GlobalEvent_Base
{
    public override string GlobalEventName => "��������� �� ������!";

    public override string Description => $"��������, ��� {targetRoad.RoadName}, ����������� ������� {targetRoad.Points[0].VillageName} " +
        $"� {targetRoad.Points[1].VillageName}, ������������ ���������� ��������. � ������� ����� ��� ������ ����� ���������� ���������.";

    [NonSerialized] public Road targetRoad;
    public string FirstPointLocationSceneName;
    public string SecondPointLocationSceneName;
    public string TargetRoadName;
    public float MultiplyCoefficient;

    public override void Execute()
    {
        DangerousRoadEventController controller = UnityEngine.Object.FindObjectOfType<DangerousRoadEventController>();
        Location firstLocation = MapManager.GetLocationBySceneName(FirstPointLocationSceneName);
        Location secondLocation = MapManager.GetLocationBySceneName(SecondPointLocationSceneName);
        targetRoad = controller.PossibleRoads
            .Where(road => 
            (road.Points[0] == firstLocation && road.Points[1] == secondLocation) ||
            (road.Points[0] == secondLocation && road.Points[1] == firstLocation))
            .FirstOrDefault(road => road.RoadName == TargetRoadName);
        targetRoad.SetRoadDangerMultiplier(MultiplyCoefficient);
    }

    public override void Terminate()
    {
        targetRoad.SetNormalDangerMultiplier();
    }
}
