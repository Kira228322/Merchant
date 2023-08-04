using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NoticeboardUI : MonoBehaviour
{
    [SerializeField] private List<Transform> _noticeSpawnPoints;
    [SerializeField] private List<GameObject> _eventNoticePrefabs;
    [SerializeField] private List<GameObject> _questNoticePrefabs;
    [SerializeField] private NoticeInformationPanel _noticeInformationPanel;
    public List<Transform> NoticeSpawnPoints => _noticeSpawnPoints;
    private Noticeboard _noticeboard;
    private List<Transform> _randomizedSpawnPoints;

    public void Initialize(Noticeboard noticeboard, Noticeboard.CompactedNotice[] compactedNotices, List<Transform> randomizedSpawnPoints)
    {
        _noticeboard = noticeboard;
        _randomizedSpawnPoints = randomizedSpawnPoints;
        for (int i = 0; i < compactedNotices.Length; i++)
        {
            if (compactedNotices[i] == null)
                continue;

            Noticeboard.CompactedNotice notice = compactedNotices[i];
            switch (notice)
            {
                case Noticeboard.CompactedQuestNotice questNotice:
                    QuestNotice newQuestNotice = AddQuestNotice(i, questNotice.name, questNotice.description, questNotice.questParams);
                    newQuestNotice.QuestGiverID = questNotice.questGiverID; 
                    break;
                case Noticeboard.CompactedEventNotice eventNotice:
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
        notice.transform.position = _randomizedSpawnPoints[spawnPointIndex].position;
        notice.Initialize(_noticeboard, name, description, spawnPointIndex);
        notice.DisplayButton.onClick.AddListener(() => OnNoticeClick(notice));
        return notice;
    }
    private QuestNotice AddQuestNotice(int spawnPointIndex, string name, string description, QuestParams questParams)
    {
        QuestNotice notice = Instantiate(_questNoticePrefabs[Random.Range(0, _questNoticePrefabs.Count)], _noticeSpawnPoints[spawnPointIndex])
            .GetComponent<QuestNotice>();
        notice.transform.position = _randomizedSpawnPoints[spawnPointIndex].position;
        notice.RandomQuest = questParams;
        notice.Initialize(_noticeboard, name, description, spawnPointIndex);
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
