using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public ItemDatabaseSO Items;
    public static ItemDatabase Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //TODO: ������ �������� ���� ���������� ��������� ����� �������
        var duplicateGroups = Instance.Items.ItemList.GroupBy(item => item.Name).Where(group => group.Count() > 1);
        foreach (var group in duplicateGroups)
        {
            Debug.LogWarning($" {group.Key} ���������� � ���� ������ ��� ����!");
        }
    }

    public static Item GetItem(string name)
    {
        Item result = Instance.Items.ItemList.FirstOrDefault(item => item.Name.ToLower() == name.ToLower());
        //��������� Linq, ����������� foreach (var item in itemlist) { if (item.Name == name) return item; else return null; } 
        if (result != null)
        {
            return result;
        }
        //���� �� ���������� ������� �� �������� �����, ������ ��� ��� ��������� ��� �������������� �������.
        //�� ����� ��������� ��������� ��� ����� ����������� �������� ��� �������� ���� -- ���� � ������ � ��������� ����� ��������� �������
        result = Instance.Items.QuestItemList.FirstOrDefault(item => item.Name.ToLower() == name.ToLower());
        if (result != null)
        {
            return result;
        }
        Debug.LogWarning($"������ ������ {name} �� ����������!");
        return null;
    }

    public static Item GetRandomItemOfThisType(Item.ItemType itemType)
    {
        List<Item> itemsOfThisType = Instance.Items.ItemList.Where(item => item.TypeOfItem == itemType).ToList();
        return itemsOfThisType[Random.Range(0, itemsOfThisType.Count)]; 
    }
    public static Item GetRandomItem()
    {
        return Instance.Items.ItemList[Random.Range(0, Instance.Items.ItemList.Count)];
    }
}
