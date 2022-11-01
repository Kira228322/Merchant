using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    // —осто€ние дороги (длительность движени€ из города ј в город Ѕ по этой дороге, ее качество, ее безопасность)
    
    [SerializeField] private float _travelingTime; // пока не€сно в каких единицах будет измер€тьс€ (игровых часах
    public float TravelingTime => _travelingTime;  // или в реальных секундах/минутах) 

    public PlaceOnMap[] Points = new PlaceOnMap[2];
}
