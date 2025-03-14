using UnityEngine;

public enum CraftingStationType { Null, Campfire, CraftingTable }
[CreateAssetMenu(fileName = "New Crafting station", menuName = "Crafting/Crafting Station")]
public class CraftingStationSO : ScriptableObject
{
    public Sprite Icon;

    public CraftingStationType type;
}
