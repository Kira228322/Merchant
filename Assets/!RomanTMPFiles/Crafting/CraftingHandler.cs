using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private UICraftingRecipe _recipeUIPrefab;
    [SerializeField] private VerticalLayoutGroup _recipesLayoutGroup;
    [SerializeField] private GameObject _recipeInformationWindow;
    [SerializeField] private Image _resultingItemIcon;
    [SerializeField] private TMP_Text _resultingItemDescription;
    [SerializeField] private TMP_Text _requiredCraftingLevelText;
    [SerializeField] private TMP_Text _currentCraftingLevelText;
    [SerializeField] private Button _craftButton;
    [SerializeField] private Image[] _plusSigns = new Image[2];
    [SerializeField] private UICraftingItemContainer[] _requiredItemContainers = new UICraftingItemContainer[3];
    [HideInInspector] public CraftingRecipe SelectedRecipe;
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        Refresh();
    }
    public void Refresh()
    {
        //Очистка после предыдущих использований
        for (int i = _recipesLayoutGroup.transform.childCount - 1; i >= 0; i--)
            Destroy(_recipesLayoutGroup.transform.GetChild(i).gameObject);
        foreach (var container in _requiredItemContainers)
            container.gameObject.SetActive(false);
        foreach (var plusSign in _plusSigns)
            plusSign.gameObject.SetActive(false);
        //Очистка завершена, можно заполнять заново

        foreach (CraftingRecipe recipe in Player.Instance.Recipes)
        //Наспавнить рецепты на панель слева
        {
            GameObject recipeUI = Instantiate(_recipeUIPrefab.gameObject, _recipesLayoutGroup.transform);
            recipeUI.GetComponent<UICraftingRecipe>().Init(this, recipe);
        }

        if (SelectedRecipe == null)
        {
            ShowCraftingElements(false);
            return;
        }

        ShowCraftingElements(true);

        _resultingItemIcon.sprite = SelectedRecipe.ResultingItem.Icon;
        _resultingItemDescription.text = SelectedRecipe.ResultingItem.Description;
        _requiredCraftingLevelText.text = "Требуемый уровень навыка: " + SelectedRecipe.RequiredCraftingLevel.ToString();
        _currentCraftingLevelText.text = "Текущий уровень навыка: " + (Player.Instance.Statistics.TotalCrafting < SelectedRecipe.RequiredCraftingLevel ? $"<color=red>" : "") + Player.Instance.Statistics.TotalCrafting;
        bool isCraftButtonEnabled = true;
        if (Player.Instance.Statistics.TotalCrafting < SelectedRecipe.RequiredCraftingLevel) 
            isCraftButtonEnabled = false;

        for (int i = 0; i < SelectedRecipe.RequiredItems.Count; i++)
        //ожидается, что i никогда не больше 2, ибо у нас всего 3 клеточки под предметы
        {
            if (i > 0) _plusSigns[i - 1].gameObject.SetActive(true);
            int currentCount = Player.Instance.Inventory.GetCount(SelectedRecipe.RequiredItems[i].item);
            if (currentCount < SelectedRecipe.RequiredItems[i].amount)
            {
                isCraftButtonEnabled = false;
            }

            _requiredItemContainers[i].gameObject.SetActive(true);
            _requiredItemContainers[i].ItemIcon.sprite = SelectedRecipe.RequiredItems[i].item.Icon;
            _requiredItemContainers[i].ItemAmount.text = $"{currentCount}/{SelectedRecipe.RequiredItems[i].amount}";

            if (currentCount >= SelectedRecipe.RequiredItems[i].amount)
                _requiredItemContainers[i].SetCompletedColor(true);
            else _requiredItemContainers[i].SetCompletedColor(false);
        }
        _craftButton.interactable = isCraftButtonEnabled;
            
    }
    public void OnCraftButtonClick()
    {
        foreach (CraftingRecipe.CraftingItem requiredItem in SelectedRecipe.RequiredItems)
        {
            Player.Instance.Inventory.RemoveItemsOfThisItemData(requiredItem.item, requiredItem.amount);
        }
        InventoryController.Instance.TryCreateAndInsertItem
            (Player.Instance.Inventory.ItemGrid, SelectedRecipe.ResultingItem, SelectedRecipe.ResultAmount, 0, true);
        Refresh();
    }
    private void ShowCraftingElements(bool value)
    {
        _recipeInformationWindow.SetActive(value);
    }
}
