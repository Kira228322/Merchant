using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GlobalEvent_DangerousRoad : GlobalEvent_Base
{
    public override string GlobalEventName => "ќпасность на дороге!";

    public override string Description => $"»звестно, что {targetRoad.RoadName}, соедин€юща€ {targetRoad.Points[0].VillageName} " +
        $"и {targetRoad.Points[1].VillageName}, подвергаетс€ нападени€м бандитов. ¬ течение суток эта дорога имеет повышенную опасность.";

    [NonSerialized] public Road targetRoad;
    public string targetRoadName;
    public float MultiplyCoefficient;

    public override void Execute()
    {
        DangerousRoadEventController controller = UnityEngine.Object.FindObjectOfType<DangerousRoadEventController>();
        targetRoad = controller.PossibleRoads.FirstOrDefault(road => road.RoadName == targetRoadName);
        targetRoad.SetRoadDangerMultiplier(MultiplyCoefficient);
    }

    public override void Terminate()
    {
        targetRoad.SetNormalDangerMultiplier();
    }
}
