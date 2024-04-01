using System.Collections;
using TMPro;
using UnityEngine;

public class EventWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _eventNameText;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Transform _contentButtons; // ������������ ������ ��� ������
    [SerializeField] private Transform _sceneContainer;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _infoButton;
    [SerializeField] private EventInTravelInfoPanel _infoPanelPrefab;
    [SerializeField] private AudioSource _audioSource;
    private EventInTravelInfoPanel _currentInfoPanel;
    private string _infoText;
    private Animator _animator;
    private TravelEventHandler _eventHandler;

    public void SetInfoButton(string text)
    {
        if (text == "")
            _infoButton.SetActive(false);
        else
        {
            _infoButton.SetActive(true);
            _infoText = text;
        }
    }

    public void OnInfoButtonClick()
    {
        if (_currentInfoPanel != null)
		{
            Destroy(_currentInfoPanel.gameObject);
			return;
		}
		
        _currentInfoPanel = Instantiate(_infoPanelPrefab.gameObject, transform)
            .GetComponent<EventInTravelInfoPanel>();
        _currentInfoPanel.Init(this);
        _currentInfoPanel.transform.position = _infoButton.transform.position;
        _currentInfoPanel._infoText.text = _infoText;
    }

    public void DestroyInfoPanel()
    {
        if (_currentInfoPanel != null)
        {
            Destroy(_currentInfoPanel.gameObject);
            _currentInfoPanel = null;
        }
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _eventHandler = FindObjectOfType<TravelEventHandler>();

    }

    public void ChangeDescription(string text)
    {
        _description.text = text;
    }

    public void Init(EventInTravel eventInTravel)
    {
        eventInTravel.Init(this);
        _eventNameText.text = eventInTravel.EventName;
        _description.text = eventInTravel.Description;

        MapManager.EventInTravelIsActive = true;

        EventInTravel travelEvent = Instantiate(eventInTravel.gameObject, _sceneContainer).GetComponent<EventInTravel>();
        travelEvent.Init(this);

        // travelEvent.gameObject.transform.localScale = 
        //     new Vector3(travelEvent.gameObject.transform.localScale.x, travelEvent.gameObject.transform.localScale.y,1);

        // Debug.Log(Screen.currentResolution.width + " " + Screen.currentResolution.height);

        travelEvent.SetButtons();

        for (int i = 0; i < travelEvent.ButtonsLabel.Count; i++)
        {
            EventInTravelButton button = Instantiate(_buttonPrefab, _contentButtons).GetComponent<EventInTravelButton>();
            button.number = i;
            button.ButtonComponent.onClick.AddListener(() => travelEvent.OnButtonClick(button.number));
            button.ButtonComponent.onClick.AddListener(() => DeleteAllButtons());
            button.ButtonComponent.onClick.AddListener(() => PlaySound());
            button.ButtonText.text = travelEvent.ButtonsLabel[i];
        }
    }

    public void PlaySound()
    {
        _audioSource.PlayOneShotWithRandomPitch();
    }

    public void DeleteAllButtons()
    {
        for (int i = 0; i < _contentButtons.childCount; i++)
            Destroy(_contentButtons.GetChild(_contentButtons.childCount - 1 - i).gameObject);

        EventInTravelButton button = Instantiate(_buttonPrefab, _contentButtons).GetComponent<EventInTravelButton>();
        button.ButtonComponent.onClick.AddListener(() => _eventHandler.EventEnd());
        button.ButtonComponent.onClick.AddListener(() => PlaySound());
        button.ButtonText.text = "����������";
    }

    public IEnumerator EventEnd()
    {
        MapManager.EventInTravelIsActive = false;
        _animator.SetTrigger("EventEnd");

        WaitForSeconds waitForSeconds = new(1);
        yield return waitForSeconds;

        for (int i = 0; i < _contentButtons.childCount; i++)
            Destroy(_contentButtons.GetChild(_contentButtons.childCount - 1 - i).gameObject);

        Destroy(_sceneContainer.GetChild(0).gameObject);
        DestroyInfoPanel();
        gameObject.SetActive(false);
    }
}
