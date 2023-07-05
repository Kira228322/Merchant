using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status Database", menuName = "Databases/Status Database")]
public class StatusDatabaseSO : ScriptableObject
{
    public List<Status> StatusList;
}
