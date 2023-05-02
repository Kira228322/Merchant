using System.Collections.Generic;
using UnityEngine;

public class Noticeboard : MonoBehaviour
{
    [SerializeField] private List<Transform> _noticeSpawnPoints;
    [SerializeField] private List<GameObject> _noticePrefabs;
    [SerializeField] private NoticeInformationPanel _noticeInformationPanel;
    private List<GlobalEvent_Base> _uncheckedActiveGlobalEvents;
    [HideInInspector] public List<Notice> Notices = new();
    private void Start() //при заходе на сцену получить список активных ивентов и создать по ним объявления
    {
        _uncheckedActiveGlobalEvents = new(GlobalEventHandler.Instance.ActiveGlobalEvents);
        if (_uncheckedActiveGlobalEvents.Count == 0) return;
        int spawnPointIndex = 0;
        while (spawnPointIndex < _noticeSpawnPoints.Count && _uncheckedActiveGlobalEvents.Count != 0)
        {
            GlobalEvent_Base randomGlobalEvent = _uncheckedActiveGlobalEvents[Random.Range(0, _uncheckedActiveGlobalEvents.Count)];
            string description = randomGlobalEvent.Description;
            string name = randomGlobalEvent.GlobalEventName;
            if (description != null) //Если у этого типа написано Description
            {
                //Создать Notice рандомного префаба, добавить ему текст.
                Notice notice = Instantiate(_noticePrefabs[Random.Range(0, _noticePrefabs.Count)], _noticeSpawnPoints[spawnPointIndex])
                    .GetComponent<Notice>();
                notice.transform.position = _noticeSpawnPoints[spawnPointIndex].position;
                notice.Initialize(name, description);
                notice.DisplayButton.onClick.AddListener(() => OnNoticeClick(notice));
                spawnPointIndex++;
            }
            _uncheckedActiveGlobalEvents.Remove(randomGlobalEvent);
        }
    }

    public void OnNoticeClick(Notice notice)
    {
        _noticeInformationPanel.DisplayNotice(notice);
    }
}
