using System;
using System.Collections.Generic;

[Serializable]
public class DiarySaveData
{
    [Serializable]
    public class EntrySaveData
    {
        public string DateTimeAcquired;
        public string Header;
        public string Text;

        public EntrySaveData(string dateTime, string header, string text)
        {
            DateTimeAcquired = dateTime;
            Header = header;
            Text = text;
        }
    }
    public List<EntrySaveData> EntriesNews;
    public List<EntrySaveData> EntriesHints;

    public DiarySaveData(List<EntrySaveData> news, List<EntrySaveData> hints)
    {
        EntriesNews = new(news);
        EntriesHints = new(hints);
    }
}
