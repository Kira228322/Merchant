using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcSaveData
{
    public int Affinity;

    public NpcSaveData(int affinity)
    {
        Affinity = affinity;
    }
}

[Serializable]
public class NpcSaveDataTrader //������������ �� NpcSaveData??
{
    //TODO � ������� 18.03
}

