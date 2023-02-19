using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [SerializeField] private GameObject _wheelGameObject;
    [SerializeField] private GameObject _bodyGameObject;
    [SerializeField] private GameObject _suspensionGameObject;

    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    private PlayerWagonStats _wagonStats;

    // private float _qualityModifier; // ���������� � PlayerWagonStats.QualityModifier

    private void Start()
    {
        _wagonStats = Player.Instance.WagonStats;
        Player.Instance.WagonStats.WagonStatsRefreshed += OnWagonStatsRefreshed;
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
        Player.Instance.WagonStats.WagonStatsRefreshed -= OnWagonStatsRefreshed;
    }

    private void OnWagonStatsRefreshed()
    {
        Wheel = _wagonStats.Wheel;
        Body = _wagonStats.Body;
        Suspension = _wagonStats.Suspension;

        _wheelGameObject.GetComponent<SpriteRenderer>().sprite = Wheel.Sprite;
        _bodyGameObject.GetComponent<SpriteRenderer>().sprite = Body.Sprite;
        _suspensionGameObject.GetComponent<SpriteRenderer>().sprite = Suspension.Sprite;


    }
}
