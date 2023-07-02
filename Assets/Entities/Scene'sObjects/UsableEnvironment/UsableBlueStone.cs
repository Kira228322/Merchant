using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableBlueStone : UsableEnvironment
{
    [SerializeField] private Status _expBuff;
    protected override bool IsFunctionalComplete()
    {
        if (Random.Range(0,2) == 0)
        {
            int expValue = 3 + Player.Instance.Experience.CurrentLevel +
                           Random.Range(0, Player.Instance.Experience.CurrentLevel);
            Player.Instance.Experience.AddExperience(expValue);
            CanvasWarningGenerator.Instance.CreateWarning("Вы получили опыт!", $"Опыта получено: {expValue}");
        }
        else
            StatusManager.Instance.AddStatusForPlayer(_expBuff);
        return true;
    }
}
