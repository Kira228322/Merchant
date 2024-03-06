using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICraftingRecipe : MonoBehaviour
{
    [System.Serializable]
    public class RequiredItem
    {
        public Image itemIcon;
        public TMP_Text itemAmount;
    }
    [SerializeField] private RequiredItem[] requiredItems = new RequiredItem[3];
    [SerializeField] private Image resultingItemIcon;
    [SerializeField] private TMP_Text _itemName;
    private CraftingHandler _craftingHandler;
    private CraftingRecipe _recipe;

    public void Init(CraftingHandler craftingHandler, CraftingRecipe recipe)
    {
        _craftingHandler = craftingHandler;
        _recipe = recipe;
        resultingItemIcon.sprite = _recipe.ResultingItem.Icon;
        _itemName.text = _recipe.ResultingItem.Name;

        for (int i = 0; i < _recipe.RequiredItems.Count; i++)
        {
            requiredItems[i].itemIcon.gameObject.SetActive(true);
            requiredItems[i].itemAmount.gameObject.SetActive(true);
            requiredItems[i].itemIcon.sprite = _recipe.RequiredItems[i].item.Icon;
            requiredItems[i].itemAmount.text = "x" + _recipe.RequiredItems[i].amount.ToString();
        }
    }
    public void OnRecipeSelection()
    {
        _craftingHandler.SelectedRecipe = _recipe;
        _craftingHandler.Refresh();
    }


}
