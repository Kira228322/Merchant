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

    [SerializeField] private EventInTravel TEST;
    private UnityAction _unityAction;

    public void Init(EventInTravel eventInTravel)
    {
        _eventNameText.text = eventInTravel.EventName;
        _description.text = eventInTravel.Description;
        EventInTravel travelEvent = Instantiate(eventInTravel.gameObject, _sceneContainer).GetComponent<EventInTravel>();
        travelEvent.SetButtons();
        for (int i = 0; i < travelEvent.ButtonsLabel.Count; i++)
        {
            EventInTravelButton button = Instantiate(_buttonPrefub, _contentButtons).GetComponent<EventInTravelButton>();
            button.number = i;
            button.ButtonComponent.onClick.AddListener(() => travelEvent.OnButtonClick(button.number));
            button.ButtonText.text = travelEvent.ButtonsLabel[i];
        }
    }

    private void Update() // РАДИ ТЕСТА, НЕ ЗАБЫТЬ УДАЛИТЬ
    {
        if (Input.GetMouseButtonDown(1))
        {
            Init(TEST);
        }
    }
}
