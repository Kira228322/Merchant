using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diary : MonoBehaviour, ISaveable<DiarySaveData>
{
    //ѕо сути, просто сохран€емый лист строк - истори€ игрока в этом мире

    public static Diary Instance;

    [SerializeField] private VerticalLayoutGroup _scrollViewContentHints;
    [SerializeField] private VerticalLayoutGroup _scrollViewContentNews;
    [SerializeField] private VerticalLayoutGroup _scrollViewContentTutorials;
    
    [SerializeField] private TMP_Text _diaryEntryPrefab;
    [SerializeField] private GameObject _tutorialEntryPrefab;

    private List<TMP_Text> _entriesNews = new();
    private List<TMP_Text> _entriesHints = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public void AddEntry(string text, bool news)
    {
        string dateTime = $"<i>ƒень {GameTime.CurrentDay}, {(GameTime.Hours < 10? "0": "")}{GameTime.Hours}:{(GameTime.Minutes < 10 ? "0" : "")}{GameTime.Minutes}: </i>";

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
            newEntry.gameObject.GetComponentInChildren<Button>().onClick.AddListener(
                () => RemoveEntry(_entriesHints, newEntry));
        }
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
        //“уториалы не сохран€ютс€, потому что они добавл€ютс€ сюда через TutorialStateTracker 

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
