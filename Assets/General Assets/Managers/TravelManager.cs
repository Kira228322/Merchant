
using TMPro;
using UnityEngine;

public static class TravelManager
{
    public static bool Travel;
    public static string TargetSceneName;
    private static int _timeScale = 15; // ������� ��� ������ 1 ������ ���. � ������� 1 ��� - 4 ������� ���
    public static int TimeScale => _timeScale;
    private static int _travelDuration;
    private static int _quality;
    private static int _danger;
    private static int _cost;
    
    private static GameObject _travelBlock;
    private static TMP_Text _travelTime;
    private static TravelTimeCounter _timeCounter;
    private static GameObject _playerIcone;

    public static void Init(GameObject trBlock, TMP_Text trTime, TravelTimeCounter timeCounter, GameObject playerIcone)
    {
        _travelBlock = trBlock;
        _travelBlock.SetActive(false);
        _travelTime = trTime;
        _timeCounter = timeCounter;
        _playerIcone = playerIcone;
    }
    public static void StartTravel(Road road)
    {
        _travelDuration = road.TravelingTime;
        _quality = road.Quality;
        _danger = road.Danger;
        _cost = road.Cost;
        
        _travelBlock.SetActive(true);
        _travelBlock.GetComponent<Animator>().SetTrigger("StartTravel");
        if (_travelDuration / 24 == 0)
            _travelTime.text = _travelDuration + " �����";
        else
            _travelTime.text = _travelDuration/24 + " ���� " + _travelDuration % 24 + " �����";

        GameTime.SetTimeScale(_timeScale);
        _timeCounter.Init(_travelDuration, road, _playerIcone);
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
