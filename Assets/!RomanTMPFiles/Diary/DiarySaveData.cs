using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiarySaveData
{
    public List<string> EntriesNews;
    public List<string> EntriesHints;

    public DiarySaveData(List<string> news, List<string> hints)
    {
        EntriesNews = new(news);
        EntriesHints = new(hints);
    }
}
