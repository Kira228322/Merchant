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
            Noticeboard.RemoveNotice(SpawnPointIndex);
        }
        else
        {
            CanvasWarningGenerator.Instance.CreateWarning("Ошибка при показе рекламы",
                "Пожалуйста, проверьте подключение к сети, чтобы посмотреть рекламу, или вернитесь позднее.");
        }
    }
}
