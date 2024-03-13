using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Databases/Item Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<Item> ItemList;
    public List<Item> QuestItemList; //предметы, которые нельзя достать рандомно
}
