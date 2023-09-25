using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryEntryDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _mainText;
    [SerializeField] private Scrollbar _scrollbar;

    public void ShowEntry(DiaryEntry entry)
    {
        _header.text = entry.Header;
        _mainText.text = entry.TextInfo;
        gameObject.SetActive(true);
        _scrollbar.value = 1;
    }
}
