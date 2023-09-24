using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnfinishedRework
{

    [CreateAssetMenu(fileName = "newItem", menuName = "Item/UsableItem")]
    public class UsableItem : Item
    {
        public enum UsableType { Edible, Potion, Bottle, Teleport, Recipe, Note, Energetic }

        [Space]
        [Header("Special settings")]
        public UsableType UsableItemType;

        [HideInInspector] public int UsableValue;
        [HideInInspector] public int SecondValue;
        [HideInInspector] public List<Item> ItemsGivenAfterUse;
        [HideInInspector] public Status Effect;
        [HideInInspector] public List<CraftingRecipe> Recipes;
        [HideInInspector] public string NoteHeader;
        [HideInInspector] public string NoteText;
    }
}