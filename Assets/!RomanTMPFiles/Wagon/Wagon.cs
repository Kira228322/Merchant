using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    private PlayerWagonStats _wagonStats;

    // private float _qualityModifier; // ���������� � PlayerWagonStats.QualityModifier

    private void Start()
    {
        _wagonStats = Player.Singleton.WagonStats;
        Player.Singleton.WagonStats.WagonStatsRefreshed += OnWagonStatsRefreshed;
        //^ ��-�� ������� ���������� �������� �� �� ����� �������� �������� �� ����� � OnEnable (��. ���� #5 https://forum.unity.com/threads/onenable-before-awake.361429/)
        //������� ������������� � ������, ����� ���� ��� Player � ��� ������� �������� Awake.
        //������������ �������� ����� ���� ���, ��� Start ����������� ������ ���� ��� ��� ������ ����� �������.
        //����� ����� ���������, ��������� �� �������������, ���� ����� ��������/��������� �����

        //�������� ������ �� Player.WagonStats

        Wheel = _wagonStats.Wheel;
        Body = _wagonStats.Body;
        Suspension = _wagonStats.Suspension;

        //_qualityModifier = Wheel.QualityModifier;
    }

    private void OnDisable()
    {
        Player.Singleton.WagonStats.WagonStatsRefreshed -= OnWagonStatsRefreshed;
    }

    private void OnWagonStatsRefreshed()
    {
        Wheel = _wagonStats.Wheel;
        Body = _wagonStats.Body;
        Suspension = _wagonStats.Suspension;

        Wheel.gameObject.GetComponent<SpriteRenderer>().sprite = Wheel.Sprite;
        Body.gameObject.GetComponent<SpriteRenderer>().sprite = Body.Sprite;
        Suspension.gameObject.GetComponent<SpriteRenderer>().sprite = Suspension.Sprite;


    }
}
