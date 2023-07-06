using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ItemInfo : MonoBehaviour
{
    #region Поля
    
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Button _splitButton;
    [SerializeField] private Button _rotateButton;
    [SerializeField] private Button _eatButton;
    [SerializeField] private ItemInfoSplitSlider _splitSliderPanel;

    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private TMP_Text _totalWeightText;
    [SerializeField] private TMP_Text _maxItemsInAStackText;
    [SerializeField] private TMP_Text _fragilityText;
    [SerializeField] private TMP_Text _averagePriceText;
    [SerializeField] private TMP_Text _daysToHalfSpoilText;
    [SerializeField] private TMP_Text _daysToSpoilText;
    [SerializeField] private TMP_Text _boughtDaysAgoText;
    [SerializeField] private TMP_Text _foodValueText;

    private Dictionary<UsableItem.UsableType, Action> _usableActions;

    private UsableItem _currentUsableItem;
    private Player _player;
    private InventoryItem _currentItemSelected;
    private ItemGrid _lastItemGridSelected;
    #endregion
    #region Методы инициализации
    private void Start()
    {
        _player = Player.Instance;
        _usableActions = new()
        {
            { UsableItem.UsableType.Edible, Eat },
            { UsableItem.UsableType.Bottle , UseBottle},
            { UsableItem.UsableType.Potion, UsePotion},
            { UsableItem.UsableType.Teleport , UseTeleport},
            { UsableItem.UsableType.Recipe, UseRecipe}
        };
    }

    public void Initialize(InventoryItem item, ItemGrid itemGrid)
    {
        _currentItemSelected = item;
        _lastItemGridSelected = itemGrid;

        //присвоение текста и иконок
        _itemIcon.sprite = item.ItemData.Icon;
        _itemName.text = item.ItemData.Name;
        _itemDescription.text = "Описание: " + item.ItemData.Description;

        if (item.CurrentItemsInAStack == 1)
        {
            _splitButton.interactable = false;
        }
        else _splitButton.interactable = true;

        if (item.ItemData.CellSizeWidth == item.ItemData.CellSizeHeight)
        {
            _rotateButton.interactable = false;
        }
        else _rotateButton.interactable = true;

        _quantityText.text = $"Количество: {item.CurrentItemsInAStack}";
        _weightText.text = $"Вес: {item.ItemData.Weight}";
        _totalWeightText.text = $"Общий вес: {item.ItemData.Weight * item.CurrentItemsInAStack}";
        _maxItemsInAStackText.text = $"Макс. количество: {item.ItemData.MaxItemsInAStack}";
        _fragilityText.text = $"Хрупкость: {item.ItemData.Fragility}";
        _averagePriceText.text = $"Средняя цена: {item.ItemData.Price}";
        
        if (item.ItemData.IsPerishable)
        {
            _daysToHalfSpoilText.alpha = 1;
            _daysToSpoilText.alpha = 1;
            _boughtDaysAgoText.alpha = 1;
            _daysToHalfSpoilText.text = $"Дней до потери свежести: {item.ItemData.DaysToHalfSpoil}";
            _daysToSpoilText.text = $"Дней до порчи: {item.ItemData.DaysToSpoil}";
            _boughtDaysAgoText.text = "Хранится уже: " + Math.Round(item.BoughtDaysAgo, 1);
        }
        else
        {
            _daysToHalfSpoilText.alpha = 0;
            _daysToSpoilText.alpha = 0;
            _boughtDaysAgoText.alpha = 0;
        }
        if (item.ItemData is UsableItem)
        {
            _currentUsableItem = _currentItemSelected.ItemData as UsableItem;
            _foodValueText.alpha = 1;
            _foodValueText.text = $"+{_currentUsableItem.UsableValue} сытости";
            _eatButton.gameObject.SetActive(true);
        }
        else
        {
            _foodValueText.alpha = 0;
            _eatButton.interactable = false;
            _eatButton.gameObject.SetActive(false);
        }
    }
    #endregion
    #region Методы работы с кнопками
    public void OnUseButtonPressed()
    {
        _usableActions[_currentUsableItem.UsableItemType]();
        //Удаление одного предмета после использования вынесено в метод RemoveOneItemAfterUse()
        //и теперь вызывается в методах словаря. 09.04.23
    }
    public void OnRotateButtonPressed()
    {
        if (_currentItemSelected.ItemData.CellSizeHeight != _currentItemSelected.ItemData.CellSizeWidth)
        {
            InventoryItem item = InventoryController.Instance.TryPickUpRotateInsert(_currentItemSelected, _lastItemGridSelected);
            if (item != null)
            {
                Destroy(gameObject);
            }
        }
    }
    public void OnSplitButtonPressed()
    {
        _splitSliderPanel.Show(_currentItemSelected);
    }

    public void OnExitButtonPressed()
    {
        Destroy(gameObject);
    }

    public void OnDestroyButtonPressed()
    {
        _lastItemGridSelected.DestroyItem(_currentItemSelected);
        Destroy(gameObject);
    }
    #endregion

    #region Методы используемых предметов

    private void Eat()
    {
        _player.Needs.RestoreHunger(_currentUsableItem.UsableValue);
        RemoveOneItemAfterUse();
    }

    private void UseBottle()
    {
        InventoryController.Instance.TryCreateAndInsertItem
            (_player.ItemGrid, ItemDatabase.GetItem("Empty bottle"), 1, 0, true);
        _player.Needs.RestoreHunger(_currentUsableItem.UsableValue);
        RemoveOneItemAfterUse();
    }

    private void UsePotion()
    {
        StatusManager.Instance.AddStatusForPlayer(_currentUsableItem.Effect);
        RemoveOneItemAfterUse();
    }

    private void UseTeleport()
    {
        //логика телепорта здесь
        RemoveOneItemAfterUse();
    }

    private void UseRecipe()
    {
        foreach (var craftRecipe in _currentUsableItem.Recipes)
        {
            if (Player.Instance.Recipes.Any(recipe => recipe.ResultingItem.Name == craftRecipe.ResultingItem.Name))
            {
                FindObjectOfType<CanvasWarningGenerator>().CreateWarning("Рецепт уже известен", 
                    $"Вы уже изучили рецепт {craftRecipe.ResultingItem.Name}");
                return;
            }
            Player.Instance.Recipes.Add(craftRecipe);
        }
        
        RemoveOneItemAfterUse();
    }

    private void RemoveOneItemAfterUse()
    {
        _currentItemSelected.CurrentItemsInAStack--;
        _quantityText.text = "Количество: " + _currentItemSelected.CurrentItemsInAStack.ToString();
        if (_currentItemSelected.CurrentItemsInAStack == 0)
        {
            _lastItemGridSelected.DestroyItem(_currentItemSelected);
            Destroy(gameObject);
        }
    }

    #endregion
    public void Split(int amountToSplit) //Мб переместить его в InventoryController?
    {
        _lastItemGridSelected.RemoveItemsFromAStack(_currentItemSelected, amountToSplit); //вроде бы предусмотрено на случай всех невозможных ситуаций через другие скрипты и свойства кнопок
        InventoryItem item = InventoryController.Instance.TryCreateAndInsertItem(_lastItemGridSelected, _currentItemSelected.ItemData, amountToSplit, _currentItemSelected.BoughtDaysAgo, isFillingStackFirst: false);
        if (item != null)
        {
            Destroy(gameObject);
        }

    }

}
