using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Serialization;

public class ItemInfo : MonoBehaviour
{
    #region Поля
    
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Button _splitButton;
    [SerializeField] private Button _rotateButton;
    [FormerlySerializedAs("_eatButton")] [SerializeField] private Button _useButton;
    [SerializeField] private Button _destroyButton;
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
    [SerializeField] private TMP_Text _itemTypeText;

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
            { UsableItem.UsableType.Potion, UsePotion},
            { UsableItem.UsableType.Teleport , UseTeleport},
            { UsableItem.UsableType.Recipe, UseRecipe},
            { UsableItem.UsableType.Note, UseNote },
            { UsableItem.UsableType.Energetic , EatEnergetic}
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
        if (item.ItemData.TypeOfItem != Item.ItemType.Null)
            _itemTypeText.text = "Категория предмета: " + Item.TranslateItemType(item.ItemData.TypeOfItem);
        else
            _itemTypeText.text = "";

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

        if (item.ItemData.IsQuestItem)
        {
            _destroyButton.interactable = false;
        }
        else _destroyButton.interactable = true;

        _quantityText.text = $"Количество: {item.CurrentItemsInAStack}";
        _weightText.text = $"Вес: {item.ItemData.Weight:F1}";
        _totalWeightText.text = $"Общий вес: {item.ItemData.Weight * item.CurrentItemsInAStack:F1}";
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
            if (_currentUsableItem.UsableItemType == UsableItem.UsableType.Edible)
            {
                _foodValueText.alpha = 1;
                if (item.BoughtDaysAgo >= item.ItemData.DaysToHalfSpoil && item.ItemData.IsPerishable)
                    _foodValueText.text = $"+<color=#F8523C>{_currentUsableItem.UsableValue/2}</color> сытости";
                else
                    _foodValueText.text = $"+{_currentUsableItem.UsableValue} сытости";
            }
            else if (_currentUsableItem.UsableItemType == UsableItem.UsableType.Energetic)
            {
                _foodValueText.alpha = 1;
                if (item.BoughtDaysAgo >= item.ItemData.DaysToHalfSpoil)
                {
                    _foodValueText.text = $"+<color=#F8523C>{_currentUsableItem.UsableValue/2}</color> сытости" +
                                          $"  +<color=#F8523C>{_currentUsableItem.SecondValue/2}</color> бодрости";
                }
                else
                    _foodValueText.text = 
                     $"+{_currentUsableItem.UsableValue} сытости  +{_currentUsableItem.SecondValue} бодрости";
            }
            else
            {
                _foodValueText.alpha = 0;
            }
            
            _useButton.gameObject.SetActive(true);
            if (item.ItemData.IsPerishable)
                if (item.BoughtDaysAgo >= item.ItemData.DaysToSpoil)
                     _useButton.interactable = false;
        }
        else
        {
            _foodValueText.alpha = 0;
            _useButton.interactable = false;
            _useButton.gameObject.SetActive(false);
        }
    }
    #endregion
    #region Методы работы с кнопками
    public void OnUseButtonPressed()
    {
        if (CanPlaceAfterUseItems())
            _usableActions[_currentUsableItem.UsableItemType]();
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
        if (_currentItemSelected.CurrentItemsInAStack == 2)
            Split(1);
        else
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
        if (_currentItemSelected.BoughtDaysAgo >= _currentItemSelected.ItemData.DaysToHalfSpoil && _currentItemSelected.ItemData.IsPerishable)
            _player.Needs.RestoreHunger(_currentUsableItem.UsableValue/2);
        else
            _player.Needs.RestoreHunger(_currentUsableItem.UsableValue);
        AfterUse();
    }

    private void EatEnergetic()
    {
        if (_currentItemSelected.BoughtDaysAgo >= _currentItemSelected.ItemData.DaysToHalfSpoil && _currentItemSelected.ItemData.IsPerishable)
        {
            _player.Needs.RestoreHunger(_currentUsableItem.UsableValue/2);
            _player.Needs.RestoreSleep(_currentUsableItem.SecondValue/2);
        }
        else
        {
            _player.Needs.RestoreHunger(_currentUsableItem.UsableValue);
            _player.Needs.RestoreSleep(_currentUsableItem.SecondValue);
        }
        AfterUse();
    }
    private void UsePotion()
    {
        StatusManager.Instance.AddStatusForPlayer(_currentUsableItem.Effect);
        AfterUse();
    }

    private void UseTeleport()
    {
        //TODO логика телепорта здесь
        AfterUse();
    }

    private void UseRecipe()
    {
        foreach (var craftRecipe in _currentUsableItem.Recipes)
        {
            if (Player.Instance.Recipes.Any(recipe => recipe.ResultingItem.Name == craftRecipe.ResultingItem.Name))
            {
                CanvasWarningGenerator.Instance.CreateWarning("Рецепт уже известен", 
                    $"Вы уже изучили рецепт {craftRecipe.ResultingItem.Name}");
                continue;
            }
            Player.Instance.Recipes.Add(craftRecipe);
        }
        
        AfterUse();
    }

    private void UseNote()
    {
        Diary.Instance.DisplayEntry(Diary.Instance.AddEntry(_currentUsableItem.NoteHeader, _currentUsableItem.NoteText, false));
        AfterUse();
    }

    private void AfterUse()
    {
        RemoveOneItem();
        if (_currentUsableItem.ItemsGivenAfterUse.Count > 0)
            AddItems();
        if (_currentUsableItem.GivesQuestAfterUse)
            AddQuest();
        Player.Instance.Inventory.OnItemUsed(_currentUsableItem);
    }
    private void RemoveOneItem()
    {
        _lastItemGridSelected.RemoveItemsFromAStack(_currentItemSelected, 1);
        _quantityText.text = "Количество: " + _currentItemSelected.CurrentItemsInAStack.ToString();

        if (_currentItemSelected.CurrentItemsInAStack == 0)
        {
            Destroy(gameObject);
        }
    }
    private void AddItems()
    {
        foreach (var item in _currentUsableItem.ItemsGivenAfterUse)
        {
            Item itemData = ItemDatabase.GetItem(item.itemName);
            int countLeftToAdd = item.amount;
            while (countLeftToAdd > itemData.MaxItemsInAStack)
            {
                InventoryController.Instance.TryCreateAndInsertItem
                (Player.Instance.ItemGrid, itemData,
                itemData.MaxItemsInAStack, item.daysBoughtAgo, true);
                countLeftToAdd -= itemData.MaxItemsInAStack;
            }
            InventoryController.Instance.TryCreateAndInsertItem
                (Player.Instance.ItemGrid, itemData, 
                countLeftToAdd, item.daysBoughtAgo, true);
        }
    }
    private void AddQuest()
    {
        QuestHandler.AddQuest(_currentUsableItem.QuestSummaryGivenAfterUse);
    }

    #endregion
    #region Методы проверки
    private bool CanPlaceAfterUseItems()
    {
        int requiredSlots = 0;

        foreach (var item in _currentUsableItem.ItemsGivenAfterUse)
        {
            Item itemData = ItemDatabase.GetItem(item.itemName);
            for (int i = 0; i < item.amount; i += itemData.MaxItemsInAStack)
            {
                requiredSlots += itemData.CellSizeWidth * itemData.CellSizeHeight;
            }
        }
        int freeSlots = Player.Instance.ItemGrid.GetFreeSlotsCount();
        if (_currentItemSelected.CurrentItemsInAStack == 1)
            //Если предмет всего 1, то после использования он удалится. Значит ещё один слот станет свободным
            freeSlots++;

        if (requiredSlots <= freeSlots)
            return true;
        CanvasWarningGenerator.Instance.CreateWarning
            ("Недостаточно места", $"Освободите место в инвентаре, чтобы получить предметы. " +
            $"Свободных клеточек необходимо: {requiredSlots - freeSlots}");
        return false;
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
