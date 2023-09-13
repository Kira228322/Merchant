using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour
{
    private TutorialHandler Instance;

    [HideInInspector] public TutorialPresentation CurrentPresentation;

    [Header("Panels")]
    [SerializeField] private GameObject _blockingPanel;
    [SerializeField] private GameObject _presentationPanel;
    [Header("Presentation Panel Content")]
    [SerializeField] private TMP_Text _presentationTitle;
    [SerializeField] private TMP_Text _presentationDescription;
    [SerializeField] private Image _presentationImage;
    [Header("Buttons")]
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _closeButton;
    [Header("Slider Counter")]
    [SerializeField] private HorizontalLayoutGroup _slideCounterLayoutGroup;
    [SerializeField] private GameObject _slideCounterElementPrefab;
    [SerializeField] private Sprite _inactiveSliderCounter;
    [SerializeField] private Sprite _activeSliderCounter;

    private int _currentSlideNumber;
    private int _currentPresentationSlidesCount;
    private List<Image> _slideCounterElements;

    public TutorialPresentation TESTPRESA;

    private void Start()
    {
        if (Instance = null)
            Instance = this;

        Init(TESTPRESA);
        ShowPresentation(true);
    }

    public void Init(TutorialPresentation presentation)
    {
        CurrentPresentation = presentation;
        _currentPresentationSlidesCount = presentation.Slides.Count;

        RectTransform slideCounterTransform = _slideCounterLayoutGroup.GetComponent<RectTransform>();

        for (int i = slideCounterTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(slideCounterTransform.GetChild(i).gameObject);
        }

        _slideCounterElements = new();
        for (int i = 0; i < _currentPresentationSlidesCount; i++)
        {
            Image slideCounterElement = Instantiate(_slideCounterElementPrefab, slideCounterTransform).GetComponent<Image>();
            _slideCounterElements.Add(slideCounterElement);
        }

        _currentSlideNumber = 0;

        _presentationTitle.text = presentation.Title;
        _presentationImage.sprite = presentation.Slides[_currentSlideNumber].Image;
        _presentationDescription.text = presentation.Slides[_currentSlideNumber].Text;

        _leftButton.interactable = false;
        if (_currentPresentationSlidesCount > 1)
            _rightButton.interactable = true;
        else
        {
            _rightButton.interactable = false;
            _closeButton.interactable = true;
        }
    }

    public void ShowPresentation(bool state)
    {
        _blockingPanel.SetActive(state);
        _presentationPanel.SetActive(state);
    }
    public void NextSlide()
    {
        ChangeSlide(_currentSlideNumber + 1);
    }
    public void PreviousSlide()
    {
        ChangeSlide(_currentSlideNumber - 1);
    }

    private void ChangeSlide(int newSlideNumber)
    {
        if (newSlideNumber < 0 || newSlideNumber >= CurrentPresentation.Slides.Count)
        {
            Debug.LogError("Ошибка в презентации!");
            ShowPresentation(false);
            return;
        }

        _slideCounterElements[_currentSlideNumber].sprite = _inactiveSliderCounter;
        _slideCounterElements[newSlideNumber].sprite = _activeSliderCounter;

        _currentSlideNumber = newSlideNumber;
        TutorialPresentation.TutorialSlide slide = CurrentPresentation.Slides[_currentSlideNumber];

        _presentationImage.sprite = slide.Image;
        _presentationDescription.text = slide.Text;

        _leftButton.interactable = _currentSlideNumber > 0;
        _rightButton.interactable = _currentSlideNumber < CurrentPresentation.Slides.Count - 1;
        _closeButton.interactable = _currentSlideNumber == CurrentPresentation.Slides.Count - 1;

    }

}
