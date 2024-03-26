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
            CanvasWarningGenerator.Instance.CreateWarning("������ ��� ������ �������",
                "����������, ��������� ����������� � ����, ����� ���������� �������, ��� ��������� �������.");
        }
    }
}
