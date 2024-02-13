using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresentationDisplayer : MonoBehaviour
{
    public static PresentationDisplayer Instance;

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

    [SerializeField] private List<TutorialPresentation> Presentations = new();

    private int _currentSlideNumber;
    private int _currentPresentationSlidesCount;
    private List<Image> _slideCounterElements;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ShowPresentation(TutorialPresentation presentation)
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
        _closeButton.interactable = false;

        ChangeSlide(0);

        ShowUIElements(true);
    }
    public void ShowPresentation(string presentationSummary)
    {
        TutorialPresentation presentation = GetPresentationBySummary(presentationSummary);
        ShowPresentation(presentation);
    }
    public TutorialPresentation GetPresentationBySummary(string summary)
    {
        TutorialPresentation presentation = Presentations.FirstOrDefault(presentation => presentation.Summary == summary);
        if (presentation == null)
        {
            Debug.LogError("Презентации с таким summary нет в листе у PresentationDisplayer");
            return null;
        }
        return presentation;
    }
    public void ShowUIElements(bool state)
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
            ShowUIElements(false);
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

        if (!_closeButton.interactable && _currentSlideNumber == CurrentPresentation.Slides.Count - 1)
            _closeButton.interactable = true;

    }

}
