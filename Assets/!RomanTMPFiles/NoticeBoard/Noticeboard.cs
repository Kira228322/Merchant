using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Noticeboard: MonoBehaviour, IPointerClickHandler
{
    //Почему не наследуется от UsableEnvironment:
    //Потому что UsableEnvironment предполагает, что у предмета есть кулдаун,
    //партиклы и изменение спрайта при использовании. Ничего из этого здесь не нужно
    //TODO: Чтобы спавнящийся префаб не спавнился два раза
    private List<GlobalEvent_Base> _uncheckedActiveGlobalEvents;

    [SerializeField] private NoticeboardUI _noticeBoardWindowPrefab;
    private float _distanceToUse = 3f;
    private Transform _canvas;

    private CompactedNotice[] _compactedNoticeArray; //Информация об объявлениях, которая будет передаваться в UI
    //Размер массива - столько, сколько возможных точек спавна в префабе NoticeboardUI.

    private void Awake()
    {
        _canvas = FindObjectOfType<CanvasWarningGenerator>().transform;
        _compactedNoticeArray = new CompactedNotice[_noticeBoardWindowPrefab.SpawnPointsCount];
    }

    private void Start() 
    {
        int spawnPointIndex = 0;
        int questsToSpawn = Random.Range(0, 3);

        for (int i = 0; i < questsToSpawn; i++)
        {
            NpcQuestGiverData questGiver = MapManager.CurrentLocation.Region.GetRandomFreeQuestGiver();
           
            if (questGiver == null)
                break;
            
            _compactedNoticeArray[spawnPointIndex] = new CompactedQuestNotice(questGiver.GiveRandomQuest());
            spawnPointIndex++;
        }

        _uncheckedActiveGlobalEvents = new(GlobalEventHandler.Instance.ActiveGlobalEvents);

        while (spawnPointIndex < _compactedNoticeArray.Length && _uncheckedActiveGlobalEvents.Count != 0)
        {
            GlobalEvent_Base randomGlobalEvent = _uncheckedActiveGlobalEvents[Random.Range(0, _uncheckedActiveGlobalEvents.Count)];
            if (randomGlobalEvent.GlobalEventName != null & randomGlobalEvent.Description != null)
            {
                _compactedNoticeArray[spawnPointIndex] = new CompactedEventNotice(randomGlobalEvent);
                spawnPointIndex++;
            }
            _uncheckedActiveGlobalEvents.Remove(randomGlobalEvent);
        }
    }

    public void RemoveNotice(int index)
    {
        _compactedNoticeArray[index] = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;

        NoticeboardUI noticeboardUI = Instantiate(_noticeBoardWindowPrefab, _canvas);
        noticeboardUI.Initialize(this, _compactedNoticeArray);
        
    }

    public abstract class CompactedNotice
    {
        public string name;
        public string description;
    }
    public class CompactedQuestNotice : CompactedNotice
    {
        public QuestParams questParams;
        public CompactedQuestNotice(QuestParams questParams)
        {
            this.questParams = questParams;
            description = questParams.description;
            name = questParams.questName;
        }
    }
    public class CompactedEventNotice : CompactedNotice
    {
        public GlobalEvent_Base globalEvent;
        public CompactedEventNotice(GlobalEvent_Base globalEvent)
        {
            this.globalEvent = globalEvent;
            description = globalEvent.Description;
            name = globalEvent.GlobalEventName;
        }
    }

}
