using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // � ���� ������ ����� ��� �������������, ������� ���������� ������ � ����� ������ ����
    // ����� ��� ��������������� �� ������ ��� ����������� �������, �� ����� ��������� 
    
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _loadScreen;
    [SerializeField] private GameObject _roadWindow;
    [SerializeField] private GameObject _villageWindow;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject TravelBlock;
    [SerializeField] private TMP_Text TravelTime;
    [SerializeField] private Timeflow Timeflow;
    [SerializeField] private TravelTimeCounter _timeCounter;
        void Start()
    {
        MapManager.Init(_travelingScene, _loadScreen, _roadWindow, _villageWindow, _canvas);
        TravelManager.Init(TravelBlock, TravelTime, _timeCounter);
        GameTime.Init(Timeflow);
    }

}
