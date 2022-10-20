using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float _timeScale;

    [SerializeField] private int hours;           //������ ��� ������������
    [SerializeField] private int minutes;         //������ ��� ������������. � GameTime ��� ���� �����
                                                  //GameTime.TimeSet(int days, int hours, int minutes, int seconds)

    private Volume _volume;

    public bool activateLights;
    [SerializeField] List<Light2D> _lights;
    void Start()
    {
        _volume = GetComponent<Volume>();

        GameTime.Hours = hours;                 //������ ��� ������������
        GameTime.Minutes = minutes;             //������ ��� ������������
    }

    void FixedUpdate()
    {
        CalcTime();
        DisplayTime();

    }

    public void CalcTime()
    {
        GameTime.Seconds += Time.fixedDeltaTime * _timeScale;

        VolumeAdjust();
    }

    public void VolumeAdjust()
    {
        if (GameTime.Hours >= 21 && GameTime.Hours < 22)
        {
            _volume.weight = (float)GameTime.Minutes / 60; //������� �� 60 ��� ���� ��� �������, ������ ����� �� ����� ����� ������ � �������.
                                                           //����� ����� ����� ������ - ����� �����

            if (activateLights == false) 
            {
                if (GameTime.Minutes > 45) //��� �� ������� - ������ 45 ������ ���� 3/4 �� ����� ����� ������
                {
                    foreach (var light in _lights)
                    {
                        light.transform.parent.gameObject.SetActive(true);
                    }
                    activateLights = true;
                }    
            }
        }


        if (GameTime.Hours >= 6 && GameTime.Hours < 7)
        {
            _volume.weight = 1 - (float)GameTime.Minutes / 60;

            if (activateLights == true)
            {
                if (GameTime.Minutes > 45)
                {
                    foreach (var light in _lights)
                    {
                        light.transform.parent.gameObject.SetActive(false);
                    }
                    activateLights = false;
                }
            }
        }
    }

    public void DisplayTime()
    {
        Debug.Log($"{GameTime.Days} ���� {GameTime.Hours} ����� {GameTime.Minutes} ����� {(int)GameTime.Seconds} ������");
    }

}