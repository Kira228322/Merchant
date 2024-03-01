using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using Random = UnityEngine.Random;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static RewardedAds Instance;

    [HideInInspector] public bool IsAdLoaded = false;

    private string androidAdID = "Rewarded_Android";
    private string iOSAdID = "Rewarded_iOS";

    [SerializeField] private List<Item> _rewardList = new ();
    private int _moneyReward = 150;
    private int _expirienceBonus = 5;
    private string adID;

    private void GiveRewardToPlayer()
    {
        int exp;
        if (StatusManager.Instance.ActiveStatuses.FirstOrDefault(status =>
                status.StatusData.StatusName == "Восхищение представлением") != null)
        {
            exp = _expirienceBonus * 5;
            Player.Instance.Experience.AddExperience(exp);
        }
        else
        {
            exp = _expirienceBonus;
            Player.Instance.Experience.AddExperience(exp);
        }
        StatusManager.Instance.AddStatusForPlayer(StatusDatabase.GetStatus("Восхищение представлением"));
        
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
                CanvasWarningGenerator.Instance.CreateWarning("Спасибо за просмотр", $"Вы получили {item.Name} в количестве {count}, а так же " +
                    $"{Convert.ToInt32(Math.Round(exp * (1 + Player.Instance.Experience.ExpGain)))} опыта");
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
        CanvasWarningGenerator.Instance.CreateWarning("Спасибо за просмотр", $"Вы получили {money} золота, " +
                                                                             $"а так же {Convert.ToInt32(Math.Round(exp * (1 + Player.Instance.Experience.ExpGain)))} опыта");
    }

    private void Awake()
    {
        adID = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOSAdID
            : androidAdID;
    }

    private void Start()
    {
        LoadAd();
        Instance = this;
    }

    public void LoadAd()
    {
        Advertisement.Load(adID, this);
    }

    public void ShowAd()
    {
        Advertisement.Show(adID, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        IsAdLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adID}: {error.ToString()} - {message}");
        IsAdLoaded = false;
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        CanvasWarningGenerator.Instance.CreateWarning("Не удалось показать рекламу", "Награда не будет выдана");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        LoadAd();
        
        if (adUnitId.Equals(adID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            GiveRewardToPlayer();
        }
    }
    
}