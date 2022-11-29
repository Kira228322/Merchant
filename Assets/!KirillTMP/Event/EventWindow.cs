using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _eventNameText;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private GameObject _liveScene; // картинка в окне окна EventWindow
    [SerializeField] private Transform _contentButtons; // родительский объект для кнопок
    [SerializeField] private GameObject _buttonPrefub;
    [SerializeField] private EventInTravel _event;
}
