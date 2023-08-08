using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Road : MonoBehaviour
{
    // Состояние дороги (длительность движения из города А в город Б по этой дороге, ее качество, ее безопасность)

    [SerializeField] private string _roadName;
    public string RoadName => _roadName;
    [SerializeField] private string _description;
    public string Description => _description;
    
    [SerializeField] private int _travelingTime; // измеряется в игровых часах
    public int TravelingTime => _travelingTime;  
    [SerializeField] [Min(25)] private int _quality;
    public int Quality => _quality;
    [SerializeField] private int _danger;
    public int Danger => _danger;
    [SerializeField] private int _cost;
    public int Cost => _cost;
    [SerializeField] private Sprite _image;
    public Sprite Image => _image;

    public Location[] Points = new Location[2];
    public List<Transform> WayPoints = new(); // путь для иконки игрока на карте
    private float[] _lengthsOfWays;
    public float[] LengthOfWays => _lengthsOfWays;
    private float _lengthOfRoad;
    public float LengthOfRoad => _lengthOfRoad;

    public float DangerMultiplier { get; private set; }
    private void Start()
    {
        _lengthsOfWays = new float[WayPoints.Count];
        for (int i = 0; i < WayPoints.Count - 1; i++)
        {
            _lengthsOfWays[i] = (WayPoints[i + 1].position - WayPoints[i].position).magnitude;
            _lengthOfRoad += _lengthsOfWays[i];
        }
    }

    public void SetRoadDangerMultiplier(float value)
    //Java?
    {
        DangerMultiplier = value;
        
        if (_danger * DangerMultiplier > 70)
            DangerMultiplier = 70f / _danger;
        
    }

    public void SetNormalDangerMultiplier()
    {
        DangerMultiplier = 1f;
    }
}
