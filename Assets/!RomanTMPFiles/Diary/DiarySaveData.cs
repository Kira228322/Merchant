using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiarySaveData
{
    public List<string> Entries;

    public DiarySaveData(List<string> entries)
    {
        Entries = new(entries);
    }
}
