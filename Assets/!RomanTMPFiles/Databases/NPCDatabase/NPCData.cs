using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newNPC", menuName = "NPCs/NPC")]
public class NPCData : ScriptableObject
{
    public string Name;

    public int ID;

    [Range(0, 100)] public int Affinity;

    public TextAsset InkJSON;
}
