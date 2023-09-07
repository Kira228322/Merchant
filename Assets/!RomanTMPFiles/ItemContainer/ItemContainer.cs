using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ItemGrid))]
public class ItemContainer : MonoBehaviour
{
    [SerializeField] private ItemGrid _containerItemGrid;
    [SerializeField] private Toggle InventoryPanelButton;
    [SerializeField] private GameObject _itemContainerPanel;

    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _requiredItemTypes;
    [SerializeField] private TMP_Text _requiredValue;

    [SerializeField] private Button _acceptButton;

    public List<Item.ItemType> RequiredItemTypes;
    public enum QuestItemsBehaviourEnum { NotQuestItems, AnyItems, OnlyQuestItems};
    public QuestItemsBehaviourEnum QuestItemsBehaviour;
    public List<InventoryItem> Items = new();
    public float CurrentWeight;
    public float RequiredWeight;
    public int CurrentAmount;
    public int RequiredAmount;
    
    


    public event UnityAction DepositSuccessful;
    public event UnityAction DepositAborted;

    private InventoryController _inventoryController;

    private void Awake()
    {
        _containerItemGrid.Init();
        _inventoryController = InventoryController.Instance;
    }

    private void OnEnable()
    {
        _containerItemGrid.ItemPlacedInTheGrid += OnItemPlacedInTheGrid;
        _containerItemGrid.ItemRemovedFromTheGrid += OnItemRemovedFromTheGrid;
        _containerItemGrid.ItemUpdated += OnItemUpdated;
    }
    private void OnDisable()
    {
        _containerItemGrid.ItemPlacedInTheGrid -= OnItemPlacedInTheGrid;
        _containerItemGrid.ItemRemovedFromTheGrid -= OnItemRemovedFromTheGrid;
        _containerItemGrid.ItemUpdated -= OnItemUpdated;
    }


    private void Init(List<Item.ItemType> acceptedItemTypes, QuestItemsBehaviourEnum questItemsBehaviour, float requiredWeight, int requiredAmount, string label)
    {
        QuestItemsBehaviour = questItemsBehaviour;
        RequiredItemTypes = new(acceptedItemTypes);
        RequiredWeight = requiredWeight;
        RequiredAmount = requiredAmount;

        _label.text = label;
        for (int i = 0; i < RequiredItemTypes.Count - 1; i++)
        {
            _requiredItemTypes.text += Item.TranslateItemType(RequiredItemTypes[i]) + "/ ";
        }
        _requiredItemTypes.text += Item.TranslateItemType(RequiredItemTypes[^1]);
        Refresh();
    }
    private void Refresh()
    {
        if (RequiredWeight > 0)
            _requiredValue.text = $"{CurrentWeight}/{RequiredWeight} кг";
        else
            _requiredValue.text = $"{CurrentAmount}/{RequiredAmount} шт";

        if (CurrentWeight >= RequiredWeight && CurrentAmount >= RequiredAmount)
        {
            _acceptButton.interactable = true;
        }
        else _acceptButton.interactable = false;
    }
    private void OnItemPlacedInTheGrid(InventoryItem item)
    {
        Items.Add(item);
        CurrentWeight += item.ItemData.Weight * item.CurrentItemsInAStack;
        CurrentAmount += item.CurrentItemsInAStack;
        Refresh();
    }
    private void OnItemRemovedFromTheGrid(InventoryItem item)
    {
        Items.Remove(item);
        CurrentWeight -= item.ItemData.Weight * item.CurrentItemsInAStack;
        CurrentAmount -= item.CurrentItemsInAStack;
        Refresh();
    }
    private void OnItemUpdated(InventoryItem item, int howManyChanged)
    {
        CurrentWeight += howManyChanged * item.ItemData.Weight;
        CurrentAmount += howManyChanged;
        Refresh();
    }
    public void Accept()
    {
            foreach (InventoryItem item in Items)
            {
                _inventoryController.DestroyItem(_containerItemGrid, item);
            }

            DepositSuccessful?.Invoke();

    }
    public void Cancel()
    {
        ItemGrid playerItemGrid = Player.Instance.ItemGrid;

        if (!_inventoryController.IsThereAvailableSpaceForInsertingMultipleItems(playerItemGrid, Items))
        {
            CanvasWarningGenerator.Instance.CreateWarning("Недостаточно места!", "Освободите место в инвентаре, чтобы вернуть предметы");
            return;
        }
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            _inventoryController.MoveFromGridToGrid(_containerItemGrid, playerItemGrid, Items[i]);
        }
        
        
        InventoryPanelButton.isOn = false;
        _itemContainerPanel.SetActive(false);
        DepositAborted?.Invoke();
    }



}
