using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WagonPart Database", menuName = "Databases/WagonPart Database")]
public class WagonPartDatabaseSO : ScriptableObject
{
    public List<WagonPart> WagonPartsList;
}
