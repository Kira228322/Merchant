using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoadWindow : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _duration;
    [SerializeField] private TMP_Text _quality;
    [SerializeField] private TMP_Text _danger;
    [SerializeField] private TMP_Text _coast;
    private int _numberOfRoad;
    private Road[] _roads;
    public void Init(List<Road> road)
    {
        _roads = new Road[road.Count];
        for (int i = 0; i < _roads.Length; i++)
            _roads[i] = road[i];
        
        _image.sprite = road[0].Image;
        _name.text = road[0].RoadName;
        _description.text = road[0].Description;
        _duration.text = "������������: " + road[0].TravelingTime;
        _quality.text = "��������: " + road[0].Quality;
        _danger.text = "���������: " + road[0].Danger;
        _coast.text = "���������: " + road[0].Coast;
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
        _duration.text = "������������: " + _roads[_numberOfRoad].TravelingTime;
        _quality.text = "��������: " + _roads[_numberOfRoad].Quality;
        _danger.text = "���������: " + _roads[_numberOfRoad].Danger;
        _coast.text = "���������: " + _roads[_numberOfRoad].Coast;
    }
}
