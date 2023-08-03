using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class Noticeboard: MonoBehaviour, IPointerClickHandler
{
    //������ �� ����������� �� UsableEnvironment:
    //������ ��� UsableEnvironment ������������, ��� � �������� ���� �������,
    //�������� � ��������� ������� ��� �������������. ������ �� ����� ����� �� �����
    //UPD 28.06.23: �� ������� ��-���� �����, �� �� ��� ���������� �������� �� UsableEnvironment

    private UniqueID _uniqueID;

    private List<GlobalEvent_Base> _uncheckedActiveGlobalEvents;

    [SerializeField] private NoticeboardUI _noticeBoardWindowPrefab;
    private float _distanceToUse = 3f;
    private int _cooldownHours = 48; //������ ����� ��������� ������ ��� � ������� �����
    private Transform _canvas;
    private CooldownHandler _cooldownHandler;

    private CompactedNotice[] _compactedNoticeArray; //���������� �� �����������, ������� ����� ������������ � UI
    //������ ������� - �������, ������� ��������� ����� ������ � ������� NoticeboardUI.

    private void Awake()
    {
        _canvas = FindObjectOfType<CanvasWarningGenerator>().transform;
        _uniqueID = GetComponent<UniqueID>();
        _cooldownHandler = FindObjectOfType<CooldownHandler>();

        _compactedNoticeArray = new CompactedNotice[_noticeBoardWindowPrefab.SpawnPointsCount];
    }

    private void Start() 
    {

        int spawnPointIndex = 0;

        //����� �������

        if (IsReadyToGiveQuest()) // ���� ����� �� �� ��
        {
            //���� � ������������� (����� ����� ����, ����� � ����������) ��� ����� � Region.cs, 37.
            //��, ��� ��� ���� �� �����, �� ����� ������� � GetRandomFreeQuestGiver()

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

        //����� �����-�-����� ���������� �� ������� ������� � ����

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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;

        NoticeboardUI noticeboardUI = Instantiate(_noticeBoardWindowPrefab, _canvas);
        noticeboardUI.Initialize(this, _compactedNoticeArray);
        
    }
    public void StartCooldown()
    {
        _cooldownHandler.Register(_uniqueID.ID, _cooldownHours);
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

}
