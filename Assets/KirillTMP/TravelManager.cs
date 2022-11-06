
using TMPro;
using UnityEngine;

public static class TravelManager
{
    public static bool Travel;
    private static int _timeScale = 12;
    
    private static int _travelDuration;
    private static int _quality;
    private static int _danger;
    private static int _cost;
    
    private static GameObject _travelBlock;
    private static TMP_Text _travelTime;
    private static TravelTimeCounter _timeCounter;

    public static void Init(GameObject trBlock, TMP_Text trTime, TravelTimeCounter timeCounter)
    {
        _travelBlock = trBlock;
        _travelBlock.SetActive(false);
        _travelTime = trTime;
        _timeCounter = timeCounter;
    }
    public static void StartTravel(Road road)
    {
        _travelDuration = road.TravelingTime;
        _quality = road.Quality;
        _danger = road.Danger;
        _cost = road.Cost;
        
        _travelBlock.SetActive(true);
        if (_travelDuration / 24 == 0)
            _travelTime.text = _travelDuration + " часов";
        else
            _travelTime.text = _travelDuration/24 + " дней " + _travelDuration % 24 + " часов";

        GameTime.SetTimeScale(_timeScale);
        _timeCounter.Init(_travelDuration);
    }

    public static void EndTravel()
    {
        GameTime.SetTimeScale(1);
    }
    
    
}
