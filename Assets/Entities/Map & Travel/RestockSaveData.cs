using System;

[Serializable]
public class RestockSaveData
{
    public int LastRestockDay;
    public RestockSaveData(int lastRestockDay)
    {
        LastRestockDay = lastRestockDay;
    }
}
