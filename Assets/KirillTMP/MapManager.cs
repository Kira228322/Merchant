using UnityEngine;

public static class MapManager
{
    private static int _currenNumberOfPlace = 0;
    public static int CurrentNumberOfPlace => _currenNumberOfPlace;
    private static string _travelingScene;
    private static SceneTransiter _loadScreen;
    private static GameObject _roadWindow;
    public static GameObject RoarWindow => _roadWindow;
    public static void Init(string travelingScene, SceneTransiter loadScreen, GameObject roadWin) // ¬ начале игры надо будет инициализировать
    {
        _travelingScene = travelingScene;
        _loadScreen = loadScreen;
        _roadWindow = roadWin;
    }

    public static void TransitionToTravelScene(PlaceOnMap placeOnMap)
    {
        placeOnMap.PlayerIsHere = true;
        _currenNumberOfPlace = placeOnMap.NumberOfPlace;
        
        _loadScreen.StartTransit(_travelingScene);
    }
}
