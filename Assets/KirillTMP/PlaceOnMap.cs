using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceOnMap : MonoBehaviour
{
    [SerializeField] private string _travelingScene; // �� ����� ����� ������� �����-�� MapManager � �������
                                                     // ����� ����� ������ �� ��� ����� ������� � ������ �������,
                                                     // ����� �� ������ ���� � ���� ������ ������� Plac'� �� �����  
    
    [SerializeField] private bool _playerIsHere; // ����� ��� ���� ����� �� ���������

    public void OnPlaceClick()
    {
        if (_playerIsHere)
            return;

        SceneManager.LoadSceneAsync(_travelingScene);
        // �������� ��������� ������ � ��������
    }
}
