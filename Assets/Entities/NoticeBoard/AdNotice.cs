using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdNotice : Notice
{
    public override void Initialize(Noticeboard noticeboard, string name, string text, int number)
    {
        Noticeboard = noticeboard;
        NoticeName = name;
        NoticeDescription = text;
        SpawnPointIndex = number;
    }

    public override void OnNoticeTake()
    {
        if (RewardedAds.Instance.IsAdLoaded)
        {
            RewardedAds.Instance.ShowAd();
            Noticeboard.RemoveNotice(SpawnPointIndex);
        }
    }
}
