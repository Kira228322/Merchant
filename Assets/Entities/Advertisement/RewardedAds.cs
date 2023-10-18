using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static RewardedAds Instance;

    private string androidAdID = "Rewarded_Android";
    private string iOSAdID = "Rewarded_iOS";

    [SerializeField] private List<Item> _rewardList = new ();
    private int _moneyReward = 150;
    private int _expirienceBonus = 5;
    private string adID;

    private void GiveRewardToPlayer()
    {
        if (Random.Range(0, 10) == 0)
        {
            GiveMoneyReward();
        }
        else
        {
            Item item = _rewardList[Random.Range(0, _rewardList.Count)];
            int count = item.Price <= 100 ? 2 : 1;
            
            if (InventoryController.Instance.TryCreateAndInsertItem(
                     ItemDatabase.GetItem(item.Name), count, 0))
            {
                Player.Instance.Experience.AddExperience(_expirienceBonus);
                CanvasWarningGenerator.Instance.CreateWarning("Спасибо за просмотр", $"Вы получили {item.Name} в количестве {count}, а так же {_expirienceBonus} опыта");
            }
            else
            {
                GiveMoneyReward();
            }
        }
    }

    private void GiveMoneyReward()
    {
        int money = _moneyReward + Random.Range(Player.Instance.Experience.CurrentLevel, Player.Instance.Experience.CurrentLevel * 10);
        Player.Instance.Money += money;
        Player.Instance.Experience.AddExperience(_expirienceBonus);
        CanvasWarningGenerator.Instance.CreateWarning("Спасибо за просмотр", $"Вы получили {money} золота, а так же {_expirienceBonus} опыта");
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
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adID}: {error.ToString()} - {message}");
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
    
    // TODO чел из видео говорил, что после того, как показало рекламу нужно грузить следующую.
    // ПРОВЕРИТЬ в билде, надо ли это делать или и так все работает
    // сейчас реклама грузится после успешного просмотра. Что будет если просмотр обломать? Нужны тесты.
}