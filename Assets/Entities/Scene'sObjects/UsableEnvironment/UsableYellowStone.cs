using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableYellowStone : UsableEnvironment
{
    [SerializeField] private Status _luckBuff;
    [SerializeField] private Status _luckDebuff;
    protected override bool IsFunctionalComplete()
    {
        int randomValue = Random.Range(0, 5);
        int value;
        switch (randomValue)
        {
            case 0:
            case 1:
                value =  3 * Player.Instance.Experience.CurrentLevel + Random.Range(10, 20 + Player.Instance.Experience.CurrentLevel);
                Player.Instance.Money += value;
                CanvasWarningGenerator.Instance.CreateWarning("Вы внезапно разбогатели", $"Вы получили {value} золота");
                break;
            case 2:
                value =  2 * Player.Instance.Experience.CurrentLevel + Random.Range(10, 16);
                if (Player.Instance.Money < value)
                {
                    CanvasWarningGenerator.Instance.CreateWarning("Вы внезапно обеднели", $"Вы потеряли {Player.Instance.Money} золота");
                    Player.Instance.Money = 0;
                }
                else
                {
                    Player.Instance.Money -= value;
                    CanvasWarningGenerator.Instance.CreateWarning("Вы внезапно обеднели", $"Вы потеряли {value} золота");
                }
                break;
            case 3:
                StatusManager.Instance.AddStatusForPlayer(_luckBuff);
                break;
            case 4:
                StatusManager.Instance.AddStatusForPlayer(_luckDebuff);
                break;
        }
        return true;
    }
}
