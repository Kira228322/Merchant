using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Databases/Item Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<Item> ItemList;
}
