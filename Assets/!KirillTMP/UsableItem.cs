using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newItem", menuName = "Item/UsableItem")]
public class UsableItem : Item
{
    public enum UsableType {Edible, Potion, Bottle, Teleport}
    public UsableType UsableItemType;
    public int UsableValue;
    public Status Effect;
}
