using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class MapManager
{
    public static Location TargetLocation;
    public static Canvas Canvas;

    public static Location CurrentLocation;
    public static Region CurrentRegion => CurrentLocation.Region;
    public static Road CurrentRoad;
    public static bool IsActiveSceneTravel;
    public static bool? Advertisement = true; // троичная логика на месте
    
    private static string _travelingScene;
    private static SceneTransiter _loadScreen;
    public static SceneTransiter SceneTransiter => _loadScreen;

    private static GameObject _roadWindow;
    public static GameObject RoadWindow => _roadWindow;
    private static GameObject _villageWindow;
    public static GameObject VillageWindow => _villageWindow;
    
    private static GameObject _playerIcon;
    public static GameObject PlayerIcon => _playerIcon;

    public static event UnityAction<Location, Location> PlayerStartedTravel;

    [HideInInspector] public static List<Window> Windows = new List<Window>();
    public static bool EventInTravelIsActive;

    public static void Init(string travelingScene, SceneTransiter loadScreen, GameObject roadWin, 
            GameObject villageWin, Canvas canvas, GameObject playerIcon, Location startLocation) 
        // В начале игры надо будет инициализировать
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

    public static void OnPlayerStartedTravel(Road targetRoad, Location targetLocation)
    {
        TargetLocation = targetLocation;
        PlayerStartedTravel?.Invoke(CurrentLocation, targetLocation);
        TransitionToTravelScene(targetRoad);
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
            Debug.LogError($"Location with this sceneName {sceneName} not found");
            return null;
        }
    }
    
    public static Location GetRandomLocation(Region region)
    {
        return region.Locations[Random.Range(0, region.Locations.Count)];
    }

    
}
