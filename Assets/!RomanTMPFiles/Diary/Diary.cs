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

    [SerializeField] private VerticalLayoutGroup _scrollViewContentHints;
    [SerializeField] private VerticalLayoutGroup _scrollViewContentNews;
    [SerializeField] private TMP_Text _diaryEntryPrefab;

    private List<TMP_Text> _entriesNews = new();
    private List<TMP_Text> _entriesHints = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public void AddEntry(string text, bool news)
    {
        string dateTime = $"<i>День {GameTime.CurrentDay}, {(GameTime.Hours < 10? "0": "")}{GameTime.Hours}:{(GameTime.Minutes < 10 ? "0" : "")}{GameTime.Minutes}: </i>";

        TMP_Text newEntry;
        if (news)
        {
            newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContentNews.transform);
            newEntry.text = dateTime + text;
            _entriesNews.Add(newEntry);
            newEntry.gameObject.GetComponentInChildren<Button>().onClick.AddListener(() => RemoveEntry(_entriesNews, newEntry));
        }
        else
        {
            newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContentHints.transform);
            newEntry.text = dateTime + text;
            _entriesHints.Add(newEntry);
            newEntry.gameObject.GetComponentInChildren<Button>().onClick.AddListener(() => RemoveEntry(_entriesHints, newEntry));
        }
    }
    public void RemoveEntry(List<TMP_Text> list, TMP_Text entry)
    {
        list.Remove(entry);
        Destroy(entry.gameObject);
    }
    private void CreateSavedEntry(string text, bool news)
    {
        TMP_Text newEntry;
        if (news)
            newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContentNews.transform);
        else 
            newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContentHints.transform);
        newEntry.text = text;
    }

    public DiarySaveData SaveData()
    {
        List<string> news = new();
        List<string> hints = new();
        foreach (var entry in _entriesNews)
        {
            news.Add(entry.text);
        }
        foreach (var entry in _entriesHints)
        {
            hints.Add(entry.text);
        }
        return new(news, hints);
    }

    public void LoadData(DiarySaveData data)
    {
        foreach (string entry in data.EntriesNews)
        {
            CreateSavedEntry(entry, true);
        }
        foreach (string entry in data.EntriesHints)
        {
            CreateSavedEntry(entry, false);
        }
    }
}
