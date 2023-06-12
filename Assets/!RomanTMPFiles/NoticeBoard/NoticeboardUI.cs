using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Noticeboard;
using Random = UnityEngine.Random;

public class NoticeboardUI : MonoBehaviour
{
    [SerializeField] private List<Transform> _noticeSpawnPoints;
    [SerializeField] private List<GameObject> _eventNoticePrefabs;
    [SerializeField] private List<GameObject> _questNoticePrefabs;
    [SerializeField] private NoticeInformationPanel _noticeInformationPanel;
    private List<GlobalEvent_Base> _uncheckedActiveGlobalEvents;
    public event UnityAction<int> NoticeTaken;
    public int SpawnPointsCount => _noticeSpawnPoints.Count;


    public void Initialize(Noticeboard.CompactedNotice[] compactedNotices)
    {
        for (int i = 0; i < compactedNotices.Length; i++)
        {
            if (compactedNotices[i] == null)
                continue;

            CompactedNotice notice = compactedNotices[i];
            switch (notice)
            {
                case CompactedQuestNotice questNotice:
                    AddQuestNotice(i, questNotice.name, questNotice.description, questNotice.questParams);
                    break;
                case CompactedEventNotice eventNotice:
                    AddEventNotice(i, eventNotice.name, eventNotice.description, eventNotice.globalEvent);
                    break;
                default:
                    Debug.LogError("Здесь нет такого типа notice");
                    break;
            }
        }
    }


    private EventNotice AddEventNotice(int spawnPointIndex, string name, string description, GlobalEvent_Base randomGlobalEvent)
    {
        EventNotice notice = Instantiate(_eventNoticePrefabs[Random.Range(0, _eventNoticePrefabs.Count)], _noticeSpawnPoints[spawnPointIndex])
            .GetComponent<EventNotice>();
        notice.transform.position = _noticeSpawnPoints[spawnPointIndex].position;
        notice.Initialize(name, description);
        notice.DisplayButton.onClick.AddListener(() => OnNoticeClick(notice));
        return notice;
    }
    private QuestNotice AddQuestNotice(int spawnPointIndex, string name, string description, QuestParams questParams)
    {
        QuestNotice notice = Instantiate(_questNoticePrefabs[Random.Range(0, _questNoticePrefabs.Count)], _noticeSpawnPoints[spawnPointIndex])
            .GetComponent<QuestNotice>();
        notice.transform.position = _noticeSpawnPoints[spawnPointIndex].position;
        notice.RandomQuest = questParams;
        notice.Initialize(name, description);
        notice.DisplayButton.onClick.AddListener(() => OnNoticeClick(notice));
        return notice;
    }

    public void OnNoticeClick(Notice notice)
    {
        _noticeInformationPanel.DisplayNotice(notice);
    }
    public void OnCloseButtonClick()
    {
        Destroy(gameObject);
    }
}
