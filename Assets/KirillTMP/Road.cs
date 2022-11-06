using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Road : MonoBehaviour
{
    // —осто€ние дороги (длительность движени€ из города ј в город Ѕ по этой дороге, ее качество, ее безопасность)

    [SerializeField] private string _roadName;
    public string RoadName => _roadName;
    [SerializeField] private string _description;
    public string Description => _description;
    
    [SerializeField] private int _travelingTime; // измер€етс€ в игровых часах
    public int TravelingTime => _travelingTime;  
    [SerializeField] private int _quality;
    public int Quality => _quality;
    [SerializeField] private int _danger;
    public int Danger => _danger;
    [SerializeField] private int _cost;
    public int Cost => _cost;
    [SerializeField] private Sprite _image;
    public Sprite Image => _image;

    public PlaceOnMap[] Points = new PlaceOnMap[2];
}
