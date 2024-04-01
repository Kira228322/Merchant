using UnityEngine;

public class UsableYellowStone : UsableEnvironment
{
    [SerializeField] private Status _luckBuff;
    [SerializeField] private Status _luckDebuff;
    [SerializeField] private Status _SpeedBuff;
    [SerializeField] private Status _SpeedDeBuff;
    protected override bool IsFunctionalComplete()
    {
        int randomValue = Random.Range(0, 10);
        int value;
        switch (randomValue)
        {
            case 9:
            case 0:
            case 1:
                value = 3 * Player.Instance.Experience.CurrentLevel + Player.Instance.Statistics.Luck.Total * 2 +
                         Random.Range(12, 22 + Player.Instance.Experience.CurrentLevel);
                if (value <= 0)
                    value = 1;
                Player.Instance.Money += value;
                CanvasWarningGenerator.Instance.CreateWarning("Вы внезапно разбогатели", $"Вы получили {value} золота");
                break;
            case 2:
                value = 2 * Player.Instance.Experience.CurrentLevel + Random.Range(12, 18) - Player.Instance.Statistics.Luck.Total;
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
            case 4:
                StatusManager.Instance.AddStatusForPlayer(_luckBuff);
                break;
            case 5:
                StatusManager.Instance.AddStatusForPlayer(_luckDebuff);
                break;
            case 6:
            case 7:
                StatusManager.Instance.AddStatusForPlayer(_SpeedBuff);
                break;
            case 8:
                StatusManager.Instance.AddStatusForPlayer(_SpeedDeBuff);
                break;
            
        }
        return true;
    }
}
