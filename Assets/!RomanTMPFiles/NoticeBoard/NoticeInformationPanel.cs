using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticeInformationPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _noticeName;
    [SerializeField] private TMP_Text _noticeDescription;
    [SerializeField] private Button _takeButton;
    private Notice _currentNotice;

    private void RefreshPanel()
    {
        if (_currentNotice == null)
        {
            ShowElements(false);
        }
        else
        {
            ShowElements(true);
        }
    }
    public void DisplayNotice(Notice notice)
    {
        _currentNotice = notice;
        RefreshPanel();
        _noticeName.text = _currentNotice.NoticeName;
        _noticeDescription.text = _currentNotice.NoticeDescription;
        _takeButton.onClick.AddListener(DestroyNotice);
    }
    private void DestroyNotice()
    {
        _currentNotice.OnNoticeTake();
        _takeButton.onClick.RemoveListener(DestroyNotice);
        Destroy(_currentNotice.gameObject);
        _currentNotice = null;
        RefreshPanel();
    }
    private void ShowElements(bool value)
    {
        _noticeName.gameObject.SetActive(value);
        _noticeDescription.gameObject.SetActive(value);
        _takeButton.gameObject.SetActive(value);
    }
}
