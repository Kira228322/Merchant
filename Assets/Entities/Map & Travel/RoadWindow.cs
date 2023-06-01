using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RoadWindow : Window
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _duration;
    [SerializeField] private TMP_Text _quality;
    [SerializeField] private TMP_Text _danger;
    [SerializeField] private TMP_Text _cost;
    private int _numberOfRoad;
    private Road[] _roads;
    private Location _place;
    public void Init(List<Road> road, Location location)
    {
        _place = location;
        _roads = new Road[road.Count];
        for (int i = 0; i < _roads.Length; i++)
            _roads[i] = road[i];
        
        _image.sprite = road[0].Image;
        _name.text = road[0].RoadName;
        _description.text = road[0].Description;
        if (_roads[_numberOfRoad].TravelingTime / 24 == 0)
            _duration.text = _roads[_numberOfRoad].TravelingTime + " часов";
        else
            _duration.text = _roads[_numberOfRoad].TravelingTime / 24 + " дней  " +
                             _roads[_numberOfRoad].TravelingTime % 24 + " часов";
        
        _quality.text = "Качество: " + road[0].Quality;
        _danger.text = "Опасность: " + road[0].Danger;
        _cost.text = "Стоимость: " + road[0].Cost;
        _numberOfRoad = 0;
    }

    public void OnNextRoadButtonClick()
    {
        if (_numberOfRoad == _roads.Length - 1)
            _numberOfRoad = 0;
        else _numberOfRoad++;
        
        _image.sprite = _roads[_numberOfRoad].Image;
        _name.text = _roads[_numberOfRoad].RoadName;
        _description.text = _roads[_numberOfRoad].Description;
        if (_roads[_numberOfRoad].TravelingTime / 24 == 0)
            _duration.text = _roads[_numberOfRoad].TravelingTime + " часов";
        else
            _duration.text = _roads[_numberOfRoad].TravelingTime / 24 + " дней  " +
                             _roads[_numberOfRoad].TravelingTime % 24 + " часов";

        _quality.text = "Качество: " + _roads[_numberOfRoad].Quality;
        _danger.text = "Опасность: " + _roads[_numberOfRoad].Danger;
        _cost.text = "Стоимость: " + _roads[_numberOfRoad].Cost;
    }

    public void OnTravelButtonClick()
    { 
        MapManager.TransitionToTravelScene(_place, _roads[_numberOfRoad]);
        MapManager.TargetSceneName = _place.SceneName;
        Destroy(gameObject);
    }
}
