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
    private PlaceOnMap _place;

    public void Init(PlaceOnMap place)
    {
        _place = place;
        _icon.sprite = place.Icon;
        _villageName.text = place.VillageName;
        _description.text = place.Description;
        if (place.NumberOfPlace == MapManager.CurrentNumberOfPlace)
        {
            _button.interactable = false;
            return;
        }

        bool related = false;
        for (int i = 0; i < place.RelatedPlaces.Count; i++)
        {
            if (MapManager.CurrentNumberOfPlace == place.RelatedPlaces[i].NumberOfPlace) // Если есть связь между городами
            {
                related = true;
                break;
            }
        }
        if (!related)
            _button.interactable = false;
    }

    public void OnChooseRoadButtonClick()
    {
        GameObject win = Instantiate(MapManager.RoadWindow, MapManager.Canvas.transform);
        win.GetComponent<RoadWindow>().Init(_place._roads, _place);
        Destroy(gameObject);
    }
}
