using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardedAds : MonoBehaviour
{
    public static RewardedAds Instance;

    [HideInInspector] public bool IsAdLoaded = false;

    [SerializeField] private List<Item> _rewardList = new();
    private int _moneyReward = 150;
    private int _expirienceBonus = 5;

    private void GiveRewardToPlayer()
    {
        int exp;
        if (StatusManager.Instance.ActiveStatuses.FirstOrDefault(status =>
                status.StatusData.StatusName == "���������� ��������������") != null)
        {
            exp = _expirienceBonus * 5;
            Player.Instance.Experience.AddExperience(exp);
        }
        else
        {
            exp = _expirienceBonus;
            Player.Instance.Experience.AddExperience(exp);
        }
        StatusManager.Instance.AddStatusForPlayer(StatusDatabase.GetStatus("���������� ��������������"));

        if (Random.Range(0, 10) == 0)
        {
            GiveMoneyReward(exp);
        }
        else
        {
            Item item = _rewardList[Random.Range(0, _rewardList.Count)];
            int count = item.Price <= 100 ? 2 : 1;

            if (InventoryController.Instance.TryCreateAndInsertItem(
                     ItemDatabase.GetItem(item.Name), count, 0))
            {
                CanvasWarningGenerator.Instance.CreateWarning("������� �� ��������", $"�� �������� {item.Name} � ���������� {count}, � ��� �� " +
                    $"{Convert.ToInt32(Math.Round(exp * (1 + Player.Instance.Experience.ExpGain)))} �����");
            }
            else
            {
                GiveMoneyReward(exp);
            }
        }
    }

    private void GiveMoneyReward(int exp)
    {
        int money = _moneyReward + Random.Range(Player.Instance.Experience.CurrentLevel, Player.Instance.Experience.CurrentLevel * 10);
        Player.Instance.Money += money;
        CanvasWarningGenerator.Instance.CreateWarning("������� �� ��������", $"�� �������� {money} ������, " +
                                                                             $"� ��� �� {Convert.ToInt32(Math.Round(exp * (1 + Player.Instance.Experience.ExpGain)))} �����");
    }

    private void Start()
    {
        Instance = this;
    }

}