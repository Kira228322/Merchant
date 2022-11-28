using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private GameObject _celestialBodies; //Сюда нужно вставить Солнце, Луну и Звёзды в виде одного геймобъекта (префаб создам)
    [SerializeField] private List<Light2D> _lights;

    private Volume _volume;
    private Transform moonAndSun;
    private Transform stars;
    private const float _convertTimeToRotation = 4f; // В сутках 24*60 = 1440 минут. 1440/360 = 4
                                                     //(каждые 4 минуты нужно поворачивать объекты на 1 градус)
    private Quaternion _currentTimeRotation = Quaternion.identity;
    private float _currentTimeDegrees = 0f; // Значение в промежутке (0;360), где 0 == 00:00, 359 == 23:56) 
    private bool _activateLights;
    private void Start()
    {
        _volume = GetComponent<Volume>();
        moonAndSun = _celestialBodies.transform.Find("Moon and Sun");
        stars = _celestialBodies.transform.Find("Stars");
        AdjustToCurrentTime();
    }

    private void Update()
    {
        AdjustToCurrentTime();
    }
    private void AdjustToCurrentTime()
    {
        _currentTimeDegrees = (GameTime.Hours * 60 + GameTime.Minutes) / _convertTimeToRotation;
        _currentTimeRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -_currentTimeDegrees);
        moonAndSun.rotation = _currentTimeRotation;
        switch (_currentTimeDegrees)
        {
            case float n when (n > 315f && n <= 360f) || (n >= 0f && n <= 75f): //полностью темно, 21:00 - 05:00
                _volume.weight = 1;
                StarsSetAlpha(stars, 1f);
                break;

            case float n when n > 75f && n <= 135f:   //рассвет, 05:00 - 09:00
                _volume.weight = 1f - ((n - 75f) / 60f);
                StarsSetAlpha(stars, 1f - (n - 75f) / 60f);
                break;

            case float n when n > 135f && n <= 255f:  //полностью светло, 09:00 - 17:00
                _volume.weight = 0;
                StarsSetAlpha(stars, 0f);
                break;

            case float n when n > 255f && n <= 315f:  //закат, 17:00 - 21:00 
                _volume.weight = (n - 255f) / 60f;
                StarsSetAlpha(stars, (n - 255f) / 60f);
                break;
        }
        if (!_activateLights)
        {
            if (_volume.weight >= 0.75f)
            {
                foreach (var light in _lights)
                {
                    light.transform.gameObject.SetActive(true);
                }
                _activateLights = true;
            }
        }
        else
        {
            if (_volume.weight <= 0.56f)
            {
                foreach (var light in _lights)
                {
                    light.transform.gameObject.SetActive(false);
                }
                _activateLights = false;
            }
        }
    }
    private void StarsSetAlpha(Transform stars, float alpha) 
    {
        foreach (SpriteRenderer star in stars.GetComponentsInChildren<SpriteRenderer>())
        {
            star.color = new Color(star.color.r, star.color.g, star.color.b, alpha);
        }
    }

}