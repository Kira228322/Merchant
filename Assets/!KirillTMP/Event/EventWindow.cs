using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject _buttonPrefub;


    public void Init(EventInTravel eventInTravel)
    {
        _eventNameText.text = eventInTravel.EventName;
        _description.text = eventInTravel.Description;
        
        if (_sceneContainer.childCount == 1)
            Destroy(_sceneContainer.GetChild(0));
        
        EventInTravel travelEvent = Instantiate(eventInTravel.gameObject, _sceneContainer).GetComponent<EventInTravel>();
        travelEvent.SetButtons();
        
        for (int i = 0; i < _contentButtons.childCount; i++)
            Destroy(_contentButtons.GetChild(_contentButtons.childCount-1));
        
        for (int i = 0; i < travelEvent.ButtonsLabel.Count; i++)
        {
            EventInTravelButton button = Instantiate(_buttonPrefub, _contentButtons).GetComponent<EventInTravelButton>();
            button.number = i;
            button.ButtonComponent.onClick.AddListener(() => travelEvent.OnButtonClick(button.number));
            button.ButtonText.text = travelEvent.ButtonsLabel[i];
        }
    }
}
