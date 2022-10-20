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

    [SerializeField] private int hours;           //Только для тестирования
    [SerializeField] private int minutes;         //Только для тестирования. В GameTime уже есть метод
                                                  //GameTime.TimeSet(int days, int hours, int minutes, int seconds)

    private Volume _volume;

    public bool activateLights;
    [SerializeField] List<Light2D> _lights;
    void Start()
    {
        _volume = GetComponent<Volume>();

        GameTime.Hours = hours;                 //Только для тестирования
        GameTime.Minutes = minutes;             //Только для тестирования
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
            _volume.weight = (float)GameTime.Minutes / 60; //Деление на 60 это пока что костыль, делить нужно на общую длину заката в минутах.
                                                           //Какая общая длина заката - решим потом

            if (activateLights == false) 
            {
                if (GameTime.Minutes > 45) //Тот же костыль - вместо 45 должно быть 3/4 от общей длины заката
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
        Debug.Log($"{GameTime.Days} день {GameTime.Hours} часов {GameTime.Minutes} минут {(int)GameTime.Seconds} секунд");
    }

}