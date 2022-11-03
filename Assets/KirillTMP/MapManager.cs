using System.Collections.Generic;
using UnityEngine;

public static class MapManager
{
    public static Canvas Canvas;
    private static int _currenNumberOfPlace = 0;
    public static int CurrentNumberOfPlace => _currenNumberOfPlace;
    private static string _travelingScene;
    private static SceneTransiter _loadScreen;
    private static GameObject _roadWindow;
    public static GameObject RoadWindow => _roadWindow;
    private static GameObject _villageWindow;
    public static GameObject VillageWindow => _villageWindow;

    [HideInInspector] public static List<Window> Windows = new List<Window>();

    public static void Init(string travelingScene, SceneTransiter loadScreen, GameObject roadWin, GameObject villageWin, Canvas canvas) 
        // ¬ начале игры надо будет инициализировать
    {
        _travelingScene = travelingScene;
        _loadScreen = loadScreen;
        _roadWindow = roadWin;
        _villageWindow = villageWin;
        Canvas = canvas;
    }

    public static void TransitionToTravelScene(PlaceOnMap placeOnMap)
    {
        _currenNumberOfPlace = placeOnMap.NumberOfPlace;
        
        _loadScreen.StartTransit(_travelingScene);
    }
}
