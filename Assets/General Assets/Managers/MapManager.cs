using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MapManager
{
    public static string TargetSceneName;
    public static Canvas Canvas;
    
    private static int _currenNumberOfPlace = 0;
    public static int CurrentNumberOfPlace => _currenNumberOfPlace;
    public static Road CurrentRoad;
    
    private static string _travelingScene;
    private static SceneTransiter _loadScreen;
    
    private static GameObject _roadWindow;
    public static GameObject RoadWindow => _roadWindow;
    private static GameObject _villageWindow;
    public static GameObject VillageWindow => _villageWindow;
    
    private static GameObject _playerIcon;
    public static GameObject PlayerIcon => _playerIcon;

    [HideInInspector] public static List<Window> Windows = new List<Window>();

    public static void Init(string travelingScene, SceneTransiter loadScreen, GameObject roadWin, 
            GameObject villageWin, Canvas canvas, GameObject playerIcon) 
        // ¬ начале игры надо будет инициализировать
    {
        _travelingScene = travelingScene;
        _loadScreen = loadScreen;
        _roadWindow = roadWin;
        _villageWindow = villageWin;
        Canvas = canvas;
        _playerIcon = playerIcon;
    }

    public static void TransitionToTravelScene(PlaceOnMap placeOnMap, Road road)
    {
        _currenNumberOfPlace = placeOnMap.NumberOfPlace;
        
        _loadScreen.StartTransit(_travelingScene, road);
    }
    
    public static void TransitionToVillageScene()
    {
        _loadScreen.StartTransit(TargetSceneName);
    }

    public static void StartTravel(Road road)
    {
        CurrentRoad = road;  
        
        _loadScreen.TimeCounter.Init(road.TravelingTime, road, _playerIcon);
        _playerIcon.transform.position = road.WayPoints[0].position;
    }
}
