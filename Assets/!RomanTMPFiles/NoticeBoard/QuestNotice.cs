using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNotice : Notice
{
    public QuestParams RandomQuest;
    public override void Initialize(string name, string text)
    {
        NoticeName = name;
        NoticeDescription = text;
    }
    public override void OnNoticeTake()
    {
        QuestHandler.AddQuest(RandomQuest);
    }
}
    