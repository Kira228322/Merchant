using TMPro;
using UnityEngine;

public class TravelTimeCounter : MonoBehaviour
{
    // Вынес отсчет времени во время сцены в отдельный скрипт, что бы не проверять кучу проверок (см update этого скрипта)
    // каждый фрейм постоянно, а проверять лишь тогда, когда это надо (во время поездки) 
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

    private void Awake()
    {
        Init(MapManager.CurrentRoad, MapManager.PlayerIcon);
        _eventHandler.OnTravelSceneEnter();
    }

    public void Init(Road road, GameObject playerIcon)
    {
        _travelingTime = road.TravelingTime;
        _duration = road.TravelingTime;
        _minutes = 0;
        enabled = true;
        _road = road;
        _playerIcon = playerIcon;

        count = 0;
        _elementRoad = _road.LengthOfRoad / _travelingTime;
        SetTravelTimeText();

        if ((_playerIcon.transform.position - _road.WayPoints[0].position).magnitude
            > (_playerIcon.transform.position - _road.WayPoints[_road.WayPoints.Count - 1].position).magnitude)
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
            _travelTime.text = _duration + " часов";
        else
            _travelTime.text = _duration / 24 + " дней " + _duration % 24 + " часов";
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
