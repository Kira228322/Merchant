using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoNotice : Notice
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
        Noticeboard.RemoveNotice(SpawnPointIndex);
    }
}
