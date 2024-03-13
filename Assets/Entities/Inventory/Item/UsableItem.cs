using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newItem", menuName = "Item/UsableItem")]
public class UsableItem : Item
{
    public enum UsableType { Edible, Potion, Teleport, Recipe, Note, Energetic }

    [Space]
    [Header("Special settings")]
    public UsableType UsableItemType;

    [HideInInspector] public int UsableValue;
    [HideInInspector] public int SecondValue;
    [HideInInspector] public bool GivesItemsAfterUse;
    [HideInInspector] public List<ItemReward> ItemsGivenAfterUse;
    [HideInInspector] public bool GivesQuestAfterUse;
    [HideInInspector] public string QuestSummaryGivenAfterUse;
    [HideInInspector] public Status Effect;
    [HideInInspector] public List<CraftingRecipe> Recipes;
    [HideInInspector] public string NoteHeader;
    [HideInInspector] public string NoteText;
}
