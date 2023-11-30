using System;
using TMPro;
using UnityEngine;

public class TravelTimeCounter : MonoBehaviour
{
    // ����� ������ ������� �� ����� ����� � ��������� ������, ��� �� �� ��������� ���� �������� (�� update ����� �������)
    // ������ ����� ���������, � ��������� ���� �����, ����� ��� ���� (�� ����� �������) 
    [SerializeField] private TMP_Text _travelTime;
    [SerializeField] private TravelEventHandler _eventHandler;
    private int _duration;
    public int Duration => _duration;
    private float _minutes;
    private Road _road;
    private GameObject _playerIcon;
    private int _travelingTime;
    private int _currentWay;
    private float _elementRoad;
    private float count;
    public static string GetLocalizedTime(int amount, bool hours)
    {
        if (amount % 100 >= 11 && amount % 100 <= 14)
        {
            return hours ? "�����" : "����";
        }

        int lastDigit = amount % 10;
        switch (lastDigit)
        {
            case 1:
                return hours ? "���" : "����";
            case 2:
            case 3:
            case 4:
                return hours ? "����" : "���";
            default:
                return hours ? "�����" : "����";
        }
    }

    public void ChangeDuraion(int addTime)
    {
        _duration += addTime;
        SetTravelTimeText();
    }

    private void Awake()
    {
        Init(MapManager.CurrentRoad, MapManager.PlayerIcon);
        _eventHandler.OnTravelSceneEnter();
    }

    public void Init(Road road, GameObject playerIcon)
    {
        _travelingTime = road.TravelingTime;
        if (Player.Instance.Inventory.IsOverencumbered)
            _duration = Convert.ToInt32(Math.Ceiling(road.TravelingTime * 1.5f));
        else 
            _duration = road.TravelingTime;
        _minutes = 0;
        enabled = true;
        _road = road;
        _playerIcon = playerIcon;

        count = 0;
        _elementRoad = _road.LengthOfRoad / _travelingTime;
        SetTravelTimeText();

        if ((_playerIcon.transform.position - _road.WayPoints[0].position).magnitude
            > (_playerIcon.transform.position - _road.WayPoints[^1].position).magnitude)
        {
            _road.WayPoints.Reverse();
            _playerIcon.transform.position = road.WayPoints[0].position;
        }
        _currentWay = 0;
    }

    private void Update()
    {
        _minutes += Time.deltaTime * GameTime.GetTimeScale();
        if (_minutes >= 60)
        {
            MoveIconOnMap();
            _minutes -= 60;
            _duration--;
            SetTravelTimeText();

            _eventHandler.StartEventIfTimerExpired();

            if (_duration == 0)
            {
                _eventHandler.FreezeTravelScene();
                _eventHandler.BreakingItemAfterJourney();
            }
        }
    }

    private void SetTravelTimeText()
    {
        if (_duration / 24 == 0)
            _travelTime.text = _duration + " " + GetLocalizedTime(_duration, true);
        else
        {
            int durationDays = _duration / 24;
            int durationHours = _duration % 24;
            _travelTime.text =
                durationDays + " " + GetLocalizedTime(durationDays, false)
                + durationHours % 24 + " " + GetLocalizedTime(durationHours, true);
        }
    }

    private void MoveIconOnMap()
    {
        count++;
        while (_elementRoad * count >= (_road.WayPoints[_currentWay + 1].position - _road.WayPoints[_currentWay].position).magnitude)
        {
            if (_currentWay == _road.LengthOfWays.Length - 2)
            {
                _playerIcon.transform.position = _road.WayPoints[_currentWay + 1].position;
                return;
            }
            count = (_elementRoad * count - (_road.WayPoints[_currentWay + 1].position - _road.WayPoints[_currentWay].position).magnitude) / _elementRoad;
            _currentWay++;

        }

        _playerIcon.transform.position = Vector3.Lerp(_road.WayPoints[_currentWay].position,
            _road.WayPoints[_currentWay + 1].position,
            _elementRoad * count / (_road.WayPoints[_currentWay + 1].position - _road.WayPoints[_currentWay].position).magnitude);
    }
}
