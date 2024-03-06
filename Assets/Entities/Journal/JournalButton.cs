using UnityEngine;
using UnityEngine.UI;

public class JournalButton : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private JournalController _controller;

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Button _button;


    public void OnSelectButton()
    {
        _button.interactable = false;
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x - 20, _rectTransform.anchoredPosition.y);
    }

    public void OnDeselectButton()
    {
        _button.interactable = true;
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x + 20, _rectTransform.anchoredPosition.y);
    }

    public void OnButtonClick()
    {
        _controller.CurrentActivePanel.SetActive(false);
        _controller.CurrentActivePanel = _panel;
        _controller.CurrentActivePanel.SetActive(true);

        _controller.CurrentActiveButton.OnDeselectButton();
        _controller.CurrentActiveButton = this;
        _controller.CurrentActiveButton.OnSelectButton();
    }
}
