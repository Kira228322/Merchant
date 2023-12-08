using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RestockSaveData
{
    public int LastRestockDay;
    public RestockSaveData(int lastRestockDay)
    {
        LastRestockDay = lastRestockDay;
    }
}
