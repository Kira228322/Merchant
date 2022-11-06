using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TravelTimeCounter : MonoBehaviour
{
    // ����� ������ ������� �� ����� ����� � ��������� ������, ��� �� �� ��������� ���� �������� (�� update ����� �������)
    // ������ ����� ���������, � ��������� ���� �����, ����� ��� ���� (�� ����� �������) 
    [SerializeField] private TMP_Text _travelTime;
    private int _duration;
    private float _minutes;
    
    public void Init(int duration)
    {
        _duration = duration;
        _minutes = 0;
        enabled = true;
    }

    private void Update() 
    {
        _minutes += Time.deltaTime * GameTime.GetTimeScale();
        if (_minutes >= 60)
        {
            _minutes -= 60;
            _duration--;
            if (_duration / 24 == 0)
                _travelTime.text = _duration + " �����";
            else
                _travelTime.text = _duration/24 + " ���� " + _duration % 24 + " �����";
            if (_duration == 0)
            {
                // TODO ������� �� �����, � ������� ����� ����
            }
        }
    }
}
