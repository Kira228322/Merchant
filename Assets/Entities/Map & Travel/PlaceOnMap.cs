using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlaceOnMap : MonoBehaviour
{
    [Header("Village information")]
    [SerializeField]private Sprite _icon;
    public Sprite Icon => _icon;
    [SerializeField] private string _villageName;
    public string VillageName => _villageName;
    [SerializeField] [TextArea(2,4)] private string _description;
    public string Description => _description;
    
    
    [Space(12)]
    [Header("Initialization")]
    public int NumberOfPlace;

    [SerializeField]private string _sceneName;
    public string SceneName => _sceneName;
    [SerializeField] private List<PlaceOnMap> _relatedPlaces;
    // места в которые можно попасть из данного места (как ребра в графе) 
    public List<PlaceOnMap> RelatedPlaces => _relatedPlaces;
    [HideInInspector] public List<Road> _roads = new();
    
    
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
        GameObject win = Instantiate(MapManager.VillageWindow, MapManager.Canvas.transform);
        win.GetComponent<VillageWindow>().Init(this);
    }
}
