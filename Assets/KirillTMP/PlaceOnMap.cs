using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceOnMap : MonoBehaviour
{
    public string _currentScene;
    public int NumberOfPlace;
    public bool PlayerIsHere; // потом будет HideInInspector и как-то надо начальную локацию плееру указать

    [SerializeField] private List<PlaceOnMap> _relatedPLaces;
    // места в которые можно попасть из данного места (как ребра в графе) 
    private List<Road> _roads = new List<Road>();

    private void Start()
    {
        Road[] roads = FindObjectsOfType<Road>();
        foreach (var road in roads)
        {
            if (road.Points[0] = this)
            {
                _roads.Add(road);
                continue;
            }
            if (road.Points[1] = this)
                _roads.Add(road);
        }
    }

    public void OnPlaceClick()
    {
        if (PlayerIsHere)
            return;
        bool related = false;
        for (int i = 0; i < _relatedPLaces.Count; i++)
        {
            if (MapManager.CurrentNumberOfPlace == _relatedPLaces[i].NumberOfPlace) // Если есть связь между городами
            {
                related = true;
                break;
            }
        }
        if (!related)
            return;

        PlayerIsHere = false;
        MapManager.TransitionToTravelScene(this);
    }
}
