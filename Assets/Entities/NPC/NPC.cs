using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public string Name { get; private set; }

    public int ID { get; private set; }

    public TextAsset InkJSON { get; private set; }
    

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
    
    public virtual void SetNPCFromData(NPCData npcData)
    {
        Name = npcData.Name;
        ID = npcData.ID;
        Affinity = npcData.Affinity;
        InkJSON = npcData.InkJSON;
    }
}
