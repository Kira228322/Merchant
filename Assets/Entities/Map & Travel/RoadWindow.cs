using System;
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
        
        int travelingTime;

        if (Player.Instance.Inventory.IsOverencumbered)
        {
            _duration.color = new Color(133/255f, 17/255f, 7/255f);
            _duration.fontStyle |= FontStyles.Underline;
            travelingTime = Convert.ToInt32(Math.Ceiling(_roads[_numberOfRoad].TravelingTime * 1.5f));
        }
        else
        {
            _duration.color = new Color(0, 34/255f, 82/255f);
            _duration.fontStyle &= ~FontStyles.Underline;
            travelingTime = _roads[_numberOfRoad].TravelingTime;
        }

        if (travelingTime / 24 == 0)
            _duration.text = travelingTime + " " + TravelTimeCounter.GetLocalizedTime(travelingTime, true);
        else
        {
            int durationDays = travelingTime / 24;
            int durationHours = travelingTime % 24;
            _duration.text = 
                durationDays + " " + TravelTimeCounter.GetLocalizedTime(durationDays, false) + 
                durationHours + " " + TravelTimeCounter.GetLocalizedTime(durationHours, true);
        }
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
        
        int travelingTime;

        if (Player.Instance.Inventory.IsOverencumbered)
        {
            _duration.color = new Color(133/255f, 17/255f, 7/255f);
            _duration.fontStyle |= FontStyles.Underline;
            travelingTime = Convert.ToInt32(Math.Ceiling(_roads[_numberOfRoad].TravelingTime * 1.5f));
        }
        else
        {
            _duration.color = new Color(0, 34/255f, 82/255f);
            _duration.fontStyle &= ~FontStyles.Underline;
            travelingTime = _roads[_numberOfRoad].TravelingTime;
        }

        if (travelingTime / 24 == 0)
            _duration.text = travelingTime + " " + TravelTimeCounter.GetLocalizedTime(travelingTime, true);
        else
        {
            int durationDays = travelingTime / 24;
            int durationHours = travelingTime % 24;
            _duration.text =
                durationDays + " " + TravelTimeCounter.GetLocalizedTime(durationDays, false) +
                durationHours + " " + TravelTimeCounter.GetLocalizedTime(durationHours, true);
        }

        _quality.text = "Качество: " + _roads[_numberOfRoad].Quality;
        _danger.text = "Опасность: " + _roads[_numberOfRoad].Danger;
        _cost.text = "Стоимость: " + _roads[_numberOfRoad].Cost;
    }

    public void OnTravelButtonClick()
    {
        if (Player.Instance.Money < _roads[_numberOfRoad].Cost)
        {
            CanvasWarningGenerator.Instance.CreateWarning("У вас недостаточно золота",
                "Для проезда по этой дороге необходимо внести оплату");
            return;
        }

        Player.Instance.Money -= _roads[_numberOfRoad].Cost;
        
        MapManager.TransitionToTravelScene(_roads[_numberOfRoad]);
        MapManager.TargetLocation = _place;
        Destroy(gameObject);
    }
}
