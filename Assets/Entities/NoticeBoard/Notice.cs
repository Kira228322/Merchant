using UnityEngine;
using UnityEngine.UI;

public abstract class Notice : MonoBehaviour
{
    //���������� ������ ����� ������, ����� �� �������, ����� ��� ������, �������� �������� ��� ����� ����������.
    //��������, ������� ����������� ����� � ����� EventNotice, QuestNotice etc.
    [HideInInspector] public string NoticeName;
    [HideInInspector] public string NoticeDescription;
    protected Noticeboard Noticeboard; //����� ����� ������ �� ����� ����������
    protected int SpawnPointIndex; //����� �����, ����� �� ����� ��� ���������� �� �����
                                   //��� ����, ����� �� ��������� �������
    public Button DisplayButton;
    public abstract void Initialize(Noticeboard noticeboard, string name, string text, int number);
    public abstract void OnNoticeTake();

}
