using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNotice : Notice
{
    public QuestParams RandomQuest;
    public override void Initialize(Noticeboard noticeboard, string name, string text, int number)
    {
        Noticeboard = noticeboard;
        NoticeName = name;
        NoticeDescription = text;
        SpawnPointIndex = number;
    }
    public override void OnNoticeTake()
    {
        QuestHandler.AddQuest(RandomQuest);
        Noticeboard.StartCooldown();
        Noticeboard.RemoveNotice(SpawnPointIndex);
    }
}
    