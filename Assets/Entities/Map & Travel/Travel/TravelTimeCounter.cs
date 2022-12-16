using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TravelTimeCounter : MonoBehaviour
{
    // Вынес отсчет времени во время сцены в отдельный скрипт, что бы не проверять кучу проверок (см update этого скрипта)
    // каждый фрейм постоянно, а проверять лишь тогда, когда это надо (во время поездки) 
    [SerializeField] private TMP_Text _travelTime;
    private int _duration;
    private float _minutes;
    private Road _road;
    private GameObject _playerIcone;
    private int _travelingTime;
    private int _currentWay;
    private int count;

    private void Awake()
    {
        Init(MapManager.CurrentRoad, MapManager.PlayerIcon);
    }

    public void Init(Road road, GameObject playerIcone)
    {
        _travelingTime = road.TravelingTime;
        _duration = road.TravelingTime;
        _minutes = 0;
        enabled = true;
        _road = road;
        _playerIcone = playerIcone;
        _currentWay = 0;
        count = 0;
        SetTravelTimeText();

        if ((_playerIcone.transform.position - _road.WayPoints[0].position).magnitude
            > (_playerIcone.transform.position - _road.WayPoints[_road.WayPoints.Count - 1].position).magnitude)
            _road.WayPoints.Reverse();
        
    }

    private void Update() 
    {
        _minutes += Time.deltaTime * GameTime.GetTimeScale();
        if (_minutes >= 60)
        {
            MoveIconeOnMap();
            _minutes -= 60;
            _duration--;
            SetTravelTimeText();
            if (_duration == 0)
            {
                GameTime.SetTimeScale(1);
                MapManager.TransitionToVillageScene();
                gameObject.SetActive(false);
            }
        }
    }

    private void SetTravelTimeText()
    {
        if (_duration / 24 == 0)
            _travelTime.text = _duration + " часов";
        else
            _travelTime.text = _duration/24 + " дней " + _duration % 24 + " часов";
    }

    private void MoveIconeOnMap()
    {
        count++;
        float elementRoad = _road.LengthOfRoad / _travelingTime;
        _playerIcone.transform.position = Vector3.Lerp(_road.WayPoints[_currentWay].position, 
            _road.WayPoints[_currentWay+1].position, elementRoad*count/_road.LengthOfWays[_currentWay]);
        if (elementRoad * count >= _road.LengthOfWays[_currentWay])
        {
            count = 0;
            _currentWay++;
            _playerIcone.transform.position = _road.WayPoints[_currentWay].position;
        }
    }
}
