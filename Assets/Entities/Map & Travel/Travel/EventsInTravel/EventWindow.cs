using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _eventNameText;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Transform _contentButtons; // родительский объект для кнопок
    [SerializeField] private Transform _sceneContainer;
    [SerializeField] private GameObject _buttonPrefab;
    private Animator _animator;
    private TravelEventHandler _eventHandler;

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
            button.ButtonText.text = travelEvent.ButtonsLabel[i];
        }
    }

    public void DeleteAllButtons()
    {
        for (int i = 0; i < _contentButtons.childCount; i++)
            Destroy(_contentButtons.GetChild(_contentButtons.childCount - 1 - i).gameObject);
        
        EventInTravelButton button = Instantiate(_buttonPrefab, _contentButtons).GetComponent<EventInTravelButton>();
        button.ButtonComponent.onClick.AddListener(() => _eventHandler.EventEnd());
        button.ButtonText.text = "Продолжить";
    }
    
    public IEnumerator EventEnd()
    {
        _animator.SetTrigger("EventEnd");

        WaitForSeconds waitForSeconds = new(1);
        yield return waitForSeconds;
        
        for (int i = 0; i < _contentButtons.childCount; i++)
            Destroy(_contentButtons.GetChild(_contentButtons.childCount - 1 - i).gameObject);
        
        Destroy(_sceneContainer.GetChild(0).gameObject);
        gameObject.SetActive(false);
    }
}
