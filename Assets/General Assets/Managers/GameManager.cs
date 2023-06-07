using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // � ���� ������ ����� ��� �������������, ������� ���������� ������ � ����� ������ ����
    // ����� ��� ��������������� �� ������ ��� ����������� �������, �� ����� ��������� 
    [SerializeField] private Canvas _canvas;
    [Header("MapManager")] 
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _loadScreen;
    [SerializeField] private GameObject _roadWindow;
    [SerializeField] private GameObject _villageWindow;
    [SerializeField] private GameObject _playerIcone;
    [SerializeField] private Location _startLocation;

    [Header("GameTime")] 
    [FormerlySerializedAs("Timeflow")] [SerializeField] private Timeflow _timeflow;

    void Start()
    {
        MapManager.Init(_travelingScene, _loadScreen, _roadWindow, _villageWindow, _canvas, _playerIcone, _startLocation);
        GameTime.Init(_timeflow);
    }
}
