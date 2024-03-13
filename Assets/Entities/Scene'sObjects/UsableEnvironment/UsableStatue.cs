using UnityEngine;

public class UsableStatue : UsableEnvironment
{
    [SerializeField] private Status _luckBuff;
    protected override bool IsFunctionalComplete()
    {
        StatusManager.Instance.AddStatusForPlayer(_luckBuff);
        return true;
    }
}
