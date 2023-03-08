using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newNPC", menuName = "NPCs/NPC")]
public class NPCData : ScriptableObject
{
    public string Name;

    public int ID;

    [Range(-100, 100)] [SerializeField] private int _affinity;

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

    public TextAsset InkJSON;
}
