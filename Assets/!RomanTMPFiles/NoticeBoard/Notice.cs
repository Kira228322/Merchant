using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    //���������� ������ ����� ������, ����� �� �������, ����� ��� ������, �������� �������� ��� ����� ����������.
    //��������, ������� ����������� ����� � ����� EventNotice, QuestNotice etc.
    [HideInInspector] public string NoticeName;
    [HideInInspector] public string NoticeDescription;
    public Button DisplayButton;

    public void Initialize(string name, string text)
    {
        NoticeName = name;
        NoticeDescription = text;
    }

}
