using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // � ���� ������ ����� ��� �������������, ������� ���������� ������ � ����� ������ ����
    // ����� ��� ��������������� �� ������ ��� ����������� �������, �� ����� ��������� 
    
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _loadScreen;
    [SerializeField] private GameObject _roadWindow;
    
    void Start()
    {
        MapManager.Init(_travelingScene, _loadScreen, _roadWindow);
    }

}
