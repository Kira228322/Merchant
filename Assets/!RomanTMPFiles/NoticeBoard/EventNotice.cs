using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNotice : Notice
{
    public override void Initialize(string name, string text)
    {
        NoticeName = name;
        NoticeDescription = text;
    }

    public override void OnNoticeTake()
    {
        //�������� � ������ ��� ���� {Day}, {Time}: {Name} {Description}
    }
}
