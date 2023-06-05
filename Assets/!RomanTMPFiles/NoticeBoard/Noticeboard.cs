using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        //TODO: ¬место while cделать так, чтобы спавнилось 0-2 квеста в зависимости от чего нибудь.
        //if (???)
        while (spawnPointIndex < _noticeSpawnPoints.Count)
        {
            GenerateQuestNotice(spawnPointIndex);
            spawnPointIndex++;
        }
        _uncheckedActiveGlobalEvents = new(GlobalEventHandler.Instance.ActiveGlobalEvents);

        //—начала заспавнились квесты (их будет от 0 до 2 примерно) 
        //“еперь нужно наспавнить просто новостей по текущим событи€м в мире, то есть информаци€ котора€ ничего не делает
        while (spawnPointIndex < _noticeSpawnPoints.Count && _uncheckedActiveGlobalEvents.Count != 0)
        {
            GlobalEvent_Base randomGlobalEvent = _uncheckedActiveGlobalEvents[Random.Range(0, _uncheckedActiveGlobalEvents.Count)];
            if (AddEventNotice(randomGlobalEvent, spawnPointIndex) != null) 
                spawnPointIndex++;
            _uncheckedActiveGlobalEvents.Remove(randomGlobalEvent);
        }
    }
    private EventNotice AddEventNotice(GlobalEvent_Base randomGlobalEvent, int spawnPointIndex)
    {
        string description = randomGlobalEvent.Description;
        string name = randomGlobalEvent.GlobalEventName;
        if (description != null) //≈сли у этого типа написано Description
        {
            //—оздать Notice рандомного префаба, добавить ему текст.
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
            Debug.LogError("¬ базе данных не было рандомного квеста");
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
