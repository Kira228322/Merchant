using UnityEngine;

public class UsableGreenStone : UsableEnvironment
{

    [SerializeField] private Status _universalBuff;
    protected override bool IsFunctionalComplete()
    {
        StatusManager.Instance.AddStatusForPlayer(_universalBuff);
        return true;
    }
}
