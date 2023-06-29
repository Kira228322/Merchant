using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diary : MonoBehaviour, ISaveable<DiarySaveData>
{
    //По сути, просто сохраняемый лист строк - история игрока в этом мире

    public static Diary Instance;

    [SerializeField] private VerticalLayoutGroup _scrollViewContent;
    [SerializeField] private TMP_Text _diaryEntryPrefab;

    private List<TMP_Text> _entries = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public void AddEntry(string text)
    {
        string dateTime = $"<i>День {GameTime.CurrentDay}, {(GameTime.Hours < 10? "0": "")}{GameTime.Hours}:{(GameTime.Minutes < 10 ? "0" : "")}{GameTime.Minutes}: </i>";

        TMP_Text newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContent.transform);
        newEntry.text = dateTime + text;
        _entries.Add(newEntry);

    }
    private void CreateSavedEntry(string text)
    {
        TMP_Text newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContent.transform);
        newEntry.text = text;
    }

    public DiarySaveData SaveData()
    {
        List<string> savedEntries = new();
        foreach (var entry in _entries)
        {
            savedEntries.Add(entry.text);
        }
        return new(savedEntries);
    }

    public void LoadData(DiarySaveData data)
    {
        foreach (string entry in data.Entries)
        {
            CreateSavedEntry(entry);
        }
    }
}
