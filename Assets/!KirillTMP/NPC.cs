using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string _name;
    public string Name => _name;

    public string ID;
      
    public int Affinity
    { 
        get => _affinity;
        set
        {
            _affinity = value;
            if (_affinity < -100)
            {
                _affinity = -100;
                return;
            }
            if (_affinity > 100)
                _affinity = 100;
        }
    }
    private int _affinity;
}
