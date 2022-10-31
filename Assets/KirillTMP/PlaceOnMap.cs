using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceOnMap : MonoBehaviour
{
    public string _currentScene;
    public int NumberOfPlace;
    public bool PlayerIsHere; // ����� ����� HideInInspector � ���-�� ���� ��������� ������� ������ �������

    [SerializeField] private List<PlaceOnMap> _relatedPLaces;
    // ����� � ������� ����� ������� �� ������� ����� (��� ����� � �����) 
    
    public void OnPlaceClick()
    {
        if (PlayerIsHere)
            return;
        bool related = false;
        for (int i = 0; i < _relatedPLaces.Count; i++)
        {
            if (MapManager.CurrentNumberOfPlace == _relatedPLaces[i].NumberOfPlace) // ���� ���� ����� ����� ��������
            {
                related = true;
                break;
            }
        }
        if (!related)
            return;

        PlayerIsHere = false;
        MapManager.TransitionToTravelScene(this);
    }
}
