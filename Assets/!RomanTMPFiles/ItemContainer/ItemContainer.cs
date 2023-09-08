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

    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _requiredItemTypesText;
    [SerializeField] private TMP_Text _requiredValueText;
    [SerializeField] private TMP_Text _itemRotThresholdText;

    [SerializeField] private Button _acceptButton;
    public enum QuestItemsBehaviourEnum { NotQuestItems, AnyItems, OnlyQuestItems};

    [HideInInspector] public List<Item.ItemType> RequiredItemTypes;
    [HideInInspector] public QuestItemsBehaviourEnum QuestItemsBehaviour;
    [HideInInspector] public List<InventoryItem> Items = new();
    [HideInInspector] public float CurrentWeight;
    [HideInInspector] public float RequiredWeight;
    [HideInInspector] public int CurrentAmount;
    [HideInInspector] public int RequiredAmount;
    [HideInInspector] public float ItemRotThreshold;
    /*
    ItemRotThreshold > 0: Только предметы свежее чем ItemRotThreshold.
    ItemRotThreshold < 0: Только предметы испорченнее чем Math.Abs(ItemRotThreshold).
    Пример: Предмет полностью портится через 10 дней после покупки (DaysUntilSpoil == 10)
    ItemRotThreshold = 0.8 => допускается если предмет лежит 2 дня или меньше
    ItemRotThreshold = -0.8 => допускается если предмет лежит 2 дня или больше
    */


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


    public void Init(List<Item.ItemType> acceptedItemTypes, QuestItemsBehaviourEnum questItemsBehaviour, float requiredWeight, int requiredAmount, float requiredRotThreshold, string label)
    {
        QuestItemsBehaviour = questItemsBehaviour;
        RequiredItemTypes = new(acceptedItemTypes);
        RequiredWeight = requiredWeight;
        RequiredAmount = requiredAmount;
        ItemRotThreshold = requiredRotThreshold;

        _labelText.text = label;
        _requiredItemTypesText.text = "";
        for (int i = 0; i < RequiredItemTypes.Count - 1; i++)
        {
            _requiredItemTypesText.text += Item.TranslateItemType(RequiredItemTypes[i]) + "/ ";
        }
        _requiredItemTypesText.text += Item.TranslateItemType(RequiredItemTypes[^1]);
        if (ItemRotThreshold != 0)
        {
            _itemRotThresholdText.gameObject.SetActive(true);
            _itemRotThresholdText.text = "Свежесть: " + (ItemRotThreshold > 0 ? ">" : "<") + -System.Math.Round((1 - ItemRotThreshold) * 100, 2) + "%";
        }
        else _itemRotThresholdText.gameObject.SetActive(false);
        Refresh();
    }
    private void Refresh()
    {
        if (RequiredWeight > 0)
            _requiredValueText.text = $"{CurrentWeight}/{RequiredWeight} кг";
        else
            _requiredValueText.text = $"{CurrentAmount}/{RequiredAmount} шт";

        if (ItemRotThreshold != 0)
        {
            _itemRotThresholdText.gameObject.SetActive(true);
            _itemRotThresholdText.text = "Свежесть: " + (ItemRotThreshold > 0 ? ">" : "<") + System.Math.Round((ItemRotThreshold) * 100, 2) + "%";
        }
        else _itemRotThresholdText.gameObject.SetActive(false);

        if (CurrentWeight >= RequiredWeight && CurrentAmount >= RequiredAmount)
        {
            _acceptButton.interactable = true;
            _requiredValueText.color = Color.green;
        }
        else
        {
            _acceptButton.interactable = false;
            _requiredValueText.color = Color.red;
        }

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
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            _inventoryController.DestroyItem(_containerItemGrid, Items[i]);
        }


        ShowItselfAndInventory(false);
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


        ShowItselfAndInventory(false);
        DepositAborted?.Invoke();
    }

    public void ShowItselfAndInventory(bool state)
    {
        InventoryPanelButton.isOn = state;
        _itemContainerPanel.SetActive(state);
    }


}
