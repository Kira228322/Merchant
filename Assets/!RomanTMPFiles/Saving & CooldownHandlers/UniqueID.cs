using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueID: MonoBehaviour 
{
    [SerializeField] private string _id = Guid.NewGuid().ToString();

    public string ID => _id;

    [ContextMenu("Generate new ID")]
    private void RegenerateGUID()
    {
        _id = Guid.NewGuid().ToString();
    }
}
