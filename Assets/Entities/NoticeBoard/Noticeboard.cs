using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class Noticeboard: MonoBehaviour, IPointerClickHandler
{
    //Почему не наследуется от UsableEnvironment:
    //Потому что UsableEnvironment предполагает, что у предмета есть кулдаун,
    //партиклы и изменение спрайта при использовании. Ничего из этого здесь не нужно
    //UPD 28.06.23: ну кулдаун всё-таки нужен, но он был реализован отдельно от UsableEnvironment
    
    private UniqueID _uniqueID;

    private List<GlobalEvent_Base> _uncheckedActiveGlobalEvents;

    [SerializeField] private NoticeboardUI _noticeBoardWindowPrefab;
    [SerializeField] private SpriteRenderer _currentBoardSprite;
    [SerializeField] private List<Sprite> _boardSprites = new ();
    [SerializeField] private InfoNoticeSO _infoNoticeSO;
    private float _distanceToUse = 3.1f;
    private int _cooldownHours = 48; //Квесты могут появиться только раз в столько часов
    private Transform _canvas;
    private CooldownHandler _cooldownHandler;
    private List<Transform> _randomizedSpawnPoints; //Копирует спавнпоинты из NoticeboardUI,
                                                    //рандомизирует их и передает обратно.
                                                    //Чтобы точки спавна оставались одинаковыми
                                                    //при каждом открытии доски

    private CompactedNotice[] _compactedNoticeArray; //Информация об объявлениях, которая будет передаваться в UI
    //Размер массива - столько, сколько возможных точек спавна в префабе NoticeboardUI.

    private void Awake()
    {
        _canvas = CanvasWarningGenerator.Instance.transform;
        _uniqueID = GetComponent<UniqueID>();
        _cooldownHandler = FindObjectOfType<CooldownHandler>();

        _compactedNoticeArray = new CompactedNotice[_noticeBoardWindowPrefab.NoticeSpawnPoints.Count];
    }

    private void Start() 
    {

        _randomizedSpawnPoints = _noticeBoardWindowPrefab.NoticeSpawnPoints;
        _randomizedSpawnPoints.Shuffle();

        int spawnPointIndex = 0;

        //Спавн квестов

        if (IsReadyToGiveQuest()) // если доска не на кд
        {
            //Абуз с квестгиверами (взять квест ртом, потом с объявления) был решен в Region.cs, 37.
            //Те, что уже есть на сцене, не могут выпасть с GetRandomFreeQuestGiver()

            int questsToSpawn = Random.Range(0, 3);
            List<NpcQuestGiverData> selectedQuestGivers = new();
            for (int i = 0; i < questsToSpawn; i++)
            {
                NpcQuestGiverData questGiver = MapManager.CurrentLocation.Region.GetRandomFreeQuestGiver();
                if (questGiver != null && !selectedQuestGivers.Contains(questGiver))
                    selectedQuestGivers.Add(questGiver);
            }
            foreach (NpcQuestGiverData questGiver in selectedQuestGivers)
            {
                _compactedNoticeArray[spawnPointIndex] = new CompactedQuestNotice(questGiver.GetRandomQuest(), questGiver.ID);
                spawnPointIndex++;
            }
        }
        //Спавн одного рекламного объявления
        if (Random.Range(0, 5) != 0) // 80% что заспавнится реклама 
        {
            string description = "";
            switch (Random.Range(0,4))
            {
                case 0:
                    description = "Таинственный странствующий цирк приглашает вас на представление! " +
                                  "Билет не стоит ни копейки! Напротив - вы сами получите подарок после представления";
                    break;
                case 1:
                    description = "Приглашение на театральное представление. Вам будет предоставлено зрелище к просмотру." +
                                  "Никаких взносов, оплат и прочего. После просмотра вы получите приз.";
                    break;
                case 2:
                    description = "Временная акция! Посмотри представление и получи подарок! Успей воспользовать!";
                    break;
                case 3:
                    description = "Если вы это читаете, то вам неслыханно повезло! Посмотрите магическое шоу совершенно бесплатно!" +
                                  "А после просмотра получите подарок!";
                    break;
            }
            _compactedNoticeArray[spawnPointIndex] = new CompactedAdNotice("Рекламное объявление", description);
            spawnPointIndex++;
        }
        
        //Спавн держи-в-курсе информации по текущим ивентам в мире

        _uncheckedActiveGlobalEvents = new(GlobalEventHandler.Instance.ActiveGlobalEvents);

        while (spawnPointIndex < _compactedNoticeArray.Length && _uncheckedActiveGlobalEvents.Count != 0)
        {
            GlobalEvent_Base randomGlobalEvent = _uncheckedActiveGlobalEvents[Random.Range(0, _uncheckedActiveGlobalEvents.Count)];
            if (randomGlobalEvent.GlobalEventName != null && randomGlobalEvent.Description != null)
            {
                _compactedNoticeArray[spawnPointIndex] = new CompactedEventNotice(randomGlobalEvent);
                spawnPointIndex++;
            }
            _uncheckedActiveGlobalEvents.Remove(randomGlobalEvent);
        }

        //Спавн бесполезной информации по типу "продам гараж"

        if(spawnPointIndex < _compactedNoticeArray.Length - 1)
        {
            int infoNoticesToAdd = Random.Range(0, _compactedNoticeArray.Length - spawnPointIndex);
            List<CompactedInfoNotice> compactedNotices = _infoNoticeSO.GetRandomNoticeInfos(infoNoticesToAdd);
            for (int i = 0; i < infoNoticesToAdd; i++)
            {
                _compactedNoticeArray[spawnPointIndex + i] = compactedNotices[i];
            }
        }

        RefreshSprite();
    }

    private bool IsReadyToGiveQuest()
    {
        var thisObjectInCooldownHandler = _cooldownHandler.ObjectsOnCooldown.FirstOrDefault(item => item.UniqueID == _uniqueID.ID);
        if (thisObjectInCooldownHandler == null)
        {
            return true;
        }
        if (thisObjectInCooldownHandler.HoursLeft <= 0)
        {
            _cooldownHandler.Unregister(_uniqueID.ID);
            return true;
        }
        return false;
    }

    public void RemoveNotice(int index)
    {
        _compactedNoticeArray[index] = null;
        RefreshSprite();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;

        NoticeboardUI noticeboardUI = Instantiate(_noticeBoardWindowPrefab, _canvas);
        noticeboardUI.Initialize(this, _compactedNoticeArray, _randomizedSpawnPoints);
        
    }
    public void StartCooldown()
    {
        _cooldownHandler.Register(_uniqueID.ID, _cooldownHours);
    }

    private void RefreshSprite()
    {
        int fullness = _compactedNoticeArray.Count(notice => notice != null);
        if (fullness > 5)
            _currentBoardSprite.sprite = _boardSprites[5];
        else
            _currentBoardSprite.sprite = _boardSprites[fullness];

    }

    public abstract class CompactedNotice
    {
        public string name;
        public string description;
    }
    public class CompactedQuestNotice : CompactedNotice
    {
        public QuestParams questParams;
        public int questGiverID;
        public CompactedQuestNotice(QuestParams questParams, int questGiverID)
        {
            this.questParams = questParams;
            this.questGiverID = questGiverID;
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
    public class CompactedInfoNotice : CompactedNotice
    {
        public CompactedInfoNotice(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
    public class CompactedAdNotice : CompactedNotice
    {
        public CompactedAdNotice(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }

}
