public class EventNotice : Notice
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
        Diary.Instance.AddEntry(NoticeName, NoticeDescription, true);
        Noticeboard.RemoveNotice(SpawnPointIndex);
    }
}
