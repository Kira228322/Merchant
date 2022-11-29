using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _eventNameText;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private GameObject _liveScene; // �������� � ���� ���� EventWindow
    [SerializeField] private Transform _contentButtons; // ������������ ������ ��� ������
    [SerializeField] private GameObject _buttonPrefub;
    [SerializeField] private EventInTravel _event;
}
