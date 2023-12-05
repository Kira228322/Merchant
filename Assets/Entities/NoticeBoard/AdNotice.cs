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
        if (false) //TODO: Проверять, если нет интернета
        {
            CanvasWarningGenerator.Instance.CreateWarning("Нет подключения к Интернету", 
                "Пожалуйста, проверьте подключение к сети, чтобы посмотреть рекламу");
        }
        else
        {
            RewardedAds.Instance.ShowAd();
            Noticeboard.RemoveNotice(SpawnPointIndex);
        }
    }
}
