using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestCheatItemGiver : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown itemSelector;
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        List<string> itemNames = new();
        foreach (Item item in ItemDatabase.Instance.Items.ItemList)
        {
            itemNames.Add(item.Name);
        }
        itemSelector.AddOptions(itemNames);

    }

    public void AddItem()
    {
        Item itemToGive = ItemDatabase.GetItem(itemSelector.captionText.text);
        InventoryController.Instance.TryCreateAndInsertItem(Player.Instance.ItemGrid, itemToGive, int.Parse(inputField.text), 0, true);
    }
}
