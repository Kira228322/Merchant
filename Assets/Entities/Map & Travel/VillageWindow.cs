using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageWindow : Window
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _villageName;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Button _button;
    private Location _place;

    public void Init(Location place)
    {
        _place = place;
        _icon.sprite = place.Icon;
        _villageName.text = place.VillageName;
        _description.text = place.Description;
        if (place == MapManager.CurrentLocation)
        {
            _button.interactable = false;
            return;
        }

        bool related = false;
        for (int i = 0; i < place.RelatedPlaces.Count; i++)
        {
            if (MapManager.CurrentLocation == place.RelatedPlaces[i]) // Если есть связь между городами
            {
                related = true;
                break;
            }
        }
        if (!related)
        {
            _button.interactable = false;
        }
    }

    public void OnChooseRoadButtonClick()
    {
        GameObject win = Instantiate(MapManager.RoadWindow, MapManager.Canvas.transform);
        List<Road> roads = new List<Road>();
        foreach (var road in _place._roads)
        {
            if (road.Points[0] == MapManager.CurrentLocation || road.Points[1] == MapManager.CurrentLocation)
                roads.Add(road);
        }
        win.GetComponent<RoadWindow>().Init(roads, _place);
        Destroy(gameObject);
    }
}
