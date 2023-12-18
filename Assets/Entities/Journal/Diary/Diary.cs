using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diary : MonoBehaviour, ISaveable<DiarySaveData>
{

    public static Diary Instance;

    [SerializeField] private VerticalLayoutGroup _scrollViewContentHints;
    [SerializeField] private VerticalLayoutGroup _scrollViewContentNews;
    [SerializeField] private VerticalLayoutGroup _scrollViewContentTutorials;


    [SerializeField] private DiaryEntryDisplayer _diaryEntryDisplayer;
    [SerializeField] private DiaryEntry _diaryEntryPrefab;
    [SerializeField] private GameObject _tutorialEntryPrefab;

    private List<DiaryEntry> _entriesNews = new();
    private List<DiaryEntry> _entriesHints = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public DiaryEntry AddEntry(string header, string text, bool news)
    {
        string dateTime = $"<i>ƒень {GameTime.CurrentDay}, {(GameTime.Hours < 10? "0": "")}{GameTime.Hours}:{(GameTime.Minutes < 10 ? "0" : "")}{GameTime.Minutes}: </i>";

        DiaryEntry newEntry;
        if (news)
        {
            newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContentNews.transform).GetComponent<DiaryEntry>();
            newEntry.Header = header;
            newEntry.TextInfo = text;
            newEntry.DateTimeAcquired = dateTime;
            newEntry.HeaderText.text = dateTime + header;
            _entriesNews.Add(newEntry);
            newEntry.GetComponent<Button>().onClick.AddListener
                (() => DisplayEntry(newEntry));
        }
        else
        {
            newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContentHints.transform);
            newEntry.Header = header;
            newEntry.TextInfo = text;
            newEntry.DateTimeAcquired = dateTime;
            newEntry.HeaderText.text = dateTime + header;
            _entriesHints.Add(newEntry);
            newEntry.GetComponent<Button>().onClick.AddListener
                (() => DisplayEntry(newEntry));
        }
        return newEntry;
    }
    public void AddTutorial(TutorialStateTracker.PresentationInfo presentationInfo)
    {
        TutorialPresentation presentation = PresentationDisplayer.Instance
            .GetPresentationBySummary(presentationInfo.PresentationSummary);

        TMP_Text newEntry = Instantiate(_tutorialEntryPrefab, _scrollViewContentTutorials.transform).GetComponent<TMP_Text>();
        newEntry.text = "ќбучение: " + presentation.Title;
        newEntry.gameObject.GetComponentInChildren<Button>().onClick.AddListener(
            () => PresentationDisplayer.Instance.ShowPresentation(presentation));
    }

    public void DisplayEntry(DiaryEntry entry)
    {
        _diaryEntryDisplayer.ShowEntry(entry);
    }
    /* 
    (24.09.23) ”брал этот метод из функционала, потому что добавил новый экран показа этих текстов.
    ≈сли всЄ-таки нужно будет их убирать, то лучше сделать разделение на квестовые 
    и не квестовые записи. ”брать можно только не квестовые.

    public void RemoveEntry(List<TMP_Text> list, TMP_Text entry)
    {
        list.Remove(entry);
        Destroy(entry.gameObject);
    }
    */
    private void CreateSavedEntry(string dateTime, string header, string text, bool news)
    {
        DiaryEntry newEntry;
        if (news)
        {
            newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContentNews.transform);
            _entriesNews.Add(newEntry);
        }
        else
        {
            newEntry = Instantiate(_diaryEntryPrefab, _scrollViewContentHints.transform);
            _entriesHints.Add(newEntry);
        }
        newEntry.HeaderText.text = dateTime + header;
        newEntry.DateTimeAcquired = dateTime;
        newEntry.Header = header;
        newEntry.TextInfo = text;
        newEntry.GetComponent<Button>().onClick.AddListener(
                () => DisplayEntry(newEntry));
    }

    public DiarySaveData SaveData()
    {
        //“уториалы не сохран€ютс€, потому что они добавл€ютс€ сюда через TutorialStateTracker 

        List<DiarySaveData.EntrySaveData> news = new();
        List<DiarySaveData.EntrySaveData> hints = new();
        foreach (var entry in _entriesNews)
        {
            news.Add(new(entry.DateTimeAcquired, entry.Header, entry.TextInfo));
        }
        foreach (var entry in _entriesHints)
        {
            hints.Add(new(entry.DateTimeAcquired, entry.Header, entry.TextInfo));
        }
        return new(news, hints);
    }

    public void LoadData(DiarySaveData data)
    {
        foreach (DiarySaveData.EntrySaveData entry in data.EntriesNews)
        {
            CreateSavedEntry(entry.DateTimeAcquired, entry.Header, entry.Text, true);
        }
        foreach (DiarySaveData.EntrySaveData entry in data.EntriesHints)
        {
            CreateSavedEntry(entry.DateTimeAcquired, entry.Header, entry.Text, false);
        }
    }
}
