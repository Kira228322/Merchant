using System;

[Serializable]
public class JournalSaveData
{
    public QuestSaveData QuestsSaveData;
    public DiarySaveData DiarySaveData;

    public JournalSaveData()
    {
        QuestsSaveData = QuestHandler.SaveQuests();
        DiarySaveData = Diary.Instance.SaveData();
    }
}
