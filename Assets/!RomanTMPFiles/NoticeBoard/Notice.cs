using UnityEngine;
using UnityEngine.UI;

public abstract class Notice : MonoBehaviour
{
    //���������� ������ ����� ������, ����� �� �������, ����� ��� ������, �������� �������� ��� ����� ����������.
    //��������, ������� ����������� ����� � ����� EventNotice, QuestNotice etc.
    [HideInInspector] public string NoticeName;
    [HideInInspector] public string NoticeDescription;
    public Button DisplayButton;

    public abstract void Initialize(string name, string text);
    public abstract void OnNoticeTake();

}
