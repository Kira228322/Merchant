using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //TODO Записать в журнал как День {Day}, {Time}: {Name} {Description}
        Noticeboard.RemoveNotice(SpawnPointIndex);
    }
}
