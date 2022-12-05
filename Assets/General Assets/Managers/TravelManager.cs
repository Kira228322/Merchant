
using TMPro;
using UnityEngine;

public static class TravelManager
{
    public static bool Travel;
    public static string TargetSceneName;
    private static int _timeScale = 15; // Игровой час длится 1 минуту ирл. В поездке 1 час - 4 секунды ирл
    public static int TimeScale => _timeScale;
    public static PlayerStats PlayerStats;
    public static int TravelDuration;
    public static int Quality;
    public static int Danger;
    public static int Cost;
    
    private static GameObject _travelBlock;
    private static TMP_Text _travelTime;
    private static TravelTimeCounter _timeCounter;
    private static GameObject _playerIcone;

    public static void Init(GameObject trBlock, TMP_Text trTime, TravelTimeCounter timeCounter, GameObject playerIcone, PlayerStats playerStats)
    {
        PlayerStats = playerStats;
        _travelBlock = trBlock;
        _travelBlock.SetActive(false);
        _travelTime = trTime;
        _timeCounter = timeCounter;
        _playerIcone = playerIcone;
    }
    public static void StartTravel(Road road)
    {
        TravelDuration = road.TravelingTime;
        Quality = road.Quality;
        Danger = road.Danger;
        Cost = road.Cost;
        
        _travelBlock.SetActive(true);
        _travelBlock.GetComponent<Animator>().SetTrigger("StartTravel");
        if (TravelDuration / 24 == 0)
            _travelTime.text = TravelDuration + " часов";
        else
            _travelTime.text = TravelDuration/24 + " дней " + TravelDuration % 24 + " часов";

        GameTime.SetTimeScale(_timeScale);
        _timeCounter.Init(TravelDuration, road, _playerIcone);
        _playerIcone.transform.position = road.WayPoints[0].position;
    }

    public static void EndTravel()
    {
        GameTime.SetTimeScale(1);
        MapManager.TransitionToVillageScene();
        Travel = false;
        _travelBlock.SetActive(false);
    }
    
    
}
