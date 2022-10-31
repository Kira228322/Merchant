using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // � ���� ������ ����� ��� �������������, ������� ���������� ������ � ����� ������ ����
    // ����� ��� ��������������� �� ������ ��� ����������� �������, �� ����� ��������� 
    
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _loadScreen;
    void Start()
    {
        MapManager.Init(_travelingScene, _loadScreen);
    }

}
