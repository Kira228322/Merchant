using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CraftingStationType { Null, Campfire}
[CreateAssetMenu(fileName = "New Crafting station", menuName = "Crafting/Crafting Station")]
public class CraftingStationSO : ScriptableObject
{ 
     public Sprite Icon;
     public string Text;
     
     public CraftingStationType type;
}
