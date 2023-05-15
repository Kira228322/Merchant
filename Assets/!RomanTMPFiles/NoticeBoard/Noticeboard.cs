using System.Collections.Generic;
using UnityEngine;

public class Noticeboard : MonoBehaviour
{
    [SerializeField] private List<Transform> _noticeSpawnPoints;
    [SerializeField] private List<GameObject> _eventNoticePrefabs;
    [SerializeField] private List<GameObject> _questNoticePrefabs;
    [SerializeField] private NoticeInformationPanel _noticeInformationPanel;
    private List<GlobalEvent_Base> _uncheckedActiveGlobalEvents;
    [HideInInspector] public List<Notice> Notices = new();
    private void Start()
    {
        int spawnPointIndex = 0;
        _uncheckedActiveGlobalEvents = new(GlobalEventHandler.Instance.ActiveGlobalEvents);
        bool isEventNotice = Random.Range(0, 1) > 0.3; //�����, ����� ���������� �������: ���������(70%) ��� ��������� (30%).

        //����� ����������, ������� � ������ ��������� ������� ������� �� ������ ������.
        //�����? �� ����� ��� �� ����� ����-���� � �� ������� 500 ��������� ������� � ����� ����������.
        //��� ��������� ���-�� ������
        //��� ���� ����������, ����� ���������� ������� ����� �������, �� ��� ����������� �����
        //����������, ����� ��������� � ����� ����������.
        //������� ���� ����� � ����� TODO: ��������� ������


    }
    private EventNotice AddEventNotice(GlobalEvent_Base randomGlobalEvent, int spawnPointIndex)
    {
        string description = randomGlobalEvent.Description;
        string name = randomGlobalEvent.GlobalEventName;
        if (description != null) //���� � ����� ���� �������� Description
        {
            //������� Notice ���������� �������, �������� ��� �����.
            EventNotice notice = Instantiate(_eventNoticePrefabs[Random.Range(0, _eventNoticePrefabs.Count)], _noticeSpawnPoints[spawnPointIndex])
                .GetComponent<EventNotice>();
            notice.transform.position = _noticeSpawnPoints[spawnPointIndex].position;
            notice.Initialize(name, description);
            notice.DisplayButton.onClick.AddListener(() => OnNoticeClick(notice));
            return notice;
        }
        return null;
    }
    private QuestNotice GenerateQuestNotice(int spawnPointIndex)
    {
        QuestNotice notice = Instantiate(_questNoticePrefabs[Random.Range(0, _questNoticePrefabs.Count)], _noticeSpawnPoints[spawnPointIndex])
            .GetComponent<QuestNotice>();
        notice.transform.position = _noticeSpawnPoints[spawnPointIndex].position;
        var randomQuest = PregenQuestDatabase.GetRandomQuest(out string name, out string description);
        if (randomQuest == null)
        {
            Debug.LogError("� ���� ������ �� ���� ���������� ������");
            return null;
        }
        notice.RandomQuest = randomQuest;
        notice.Initialize(name, description);
        notice.DisplayButton.onClick.AddListener(() => OnNoticeClick(notice));
        return notice;
    }

    public void OnNoticeClick(Notice notice)
    {
        _noticeInformationPanel.DisplayNotice(notice);
    }
}
