using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestCheatItemGiver : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown itemSelector;
    [SerializeField] private TMP_InputField itemInputField;
    [SerializeField] private TMP_InputField moneyInputField;

    private void Start()
    {
        List<string> itemNames = new();
        foreach (Item item in ItemDatabase.Instance.Items.ItemList)
        {
            itemNames.Add(item.Name);
        }
        foreach (Item item in ItemDatabase.Instance.Items.QuestItemList)
        {
            itemNames.Add(item.Name);
        }
        itemSelector.AddOptions(itemNames);

    }

    public void AddItem()
    {
        Item itemToGive = ItemDatabase.GetItem(itemSelector.captionText.text);
        int amountToGive = 1;
        if (int.TryParse(itemInputField.text, out int number))
        {
            amountToGive = number;
        }
        InventoryController.Instance.TryCreateAndInsertItem(Player.Instance.ItemGrid, itemToGive, amountToGive, 0, true);
    }
    public void SetMoney()
    {
        int amountToSet = 5000;
        if (int.TryParse(moneyInputField.text, out int number))
        {
            amountToSet = number;
        }
        Player.Instance.Money = amountToSet;
    }
}
