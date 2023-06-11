using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MapManager
{
    public static Location TargetLocation;
    public static Canvas Canvas;

    public static Location CurrentLocation;
    public static Road CurrentRoad;
    public static bool IsActiveSceneTravel;
    
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
            GameObject villageWin, Canvas canvas, GameObject playerIcon, Location startLocation) 
        // ¬ начале игры надо будет инициализировать
    {
        _travelingScene = travelingScene;
        _loadScreen = loadScreen;
        _roadWindow = roadWin;
        _villageWindow = villageWin;
        Canvas = canvas;
        _playerIcon = playerIcon;
        CurrentLocation = startLocation;
    }

    public static void TransitionToTravelScene(Road road)
    {
        _loadScreen.StartTransit(_travelingScene, road);
    }
    public static void TransitionToVillageScene()
    {
        _loadScreen.StartTransit(TargetLocation);
    }

    public static void TravelInit(Road road)
    {
        CurrentRoad = road;
    }

    public static void OnLocationChange()
    {
        CurrentLocation.CountAllItemsOnScene();
        CurrentLocation.Region.CountAllItemsInRegion();
    }

    public static Location GetLocationBySceneName(string sceneName)
    {
        RegionHandler regionHandler = Object.FindObjectOfType<RegionHandler>(true);

        Location location = regionHandler.Regions
            .SelectMany(region => region.Locations)
            .FirstOrDefault(location => location.SceneName == sceneName);

        if (location != null)
        {
            return location;
        }
        else
        {
            Debug.LogError("Location with this sceneName not found");
            return null;
        }
    }
}
