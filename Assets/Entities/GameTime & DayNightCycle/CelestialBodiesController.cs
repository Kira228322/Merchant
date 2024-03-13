using UnityEngine;

public class CelestialBodiesController : MonoBehaviour
{
    private Transform moonAndSun;
    private Transform stars;
    private const float _convertTimeToRotation = 4f; // ¬ сутках 24*60 = 1440 минут. 1440/360 = 4
                                                     //(каждые 4 минуты нужно поворачивать объекты на 1 градус)
    private float _currentTimeDegrees = 0f; // «начение в промежутке (0;360), где 0 == 00:00, 359 == 23:56) 
    private float _seconds;
    private int _checkMinute;
    private void Start()
    {
        moonAndSun = transform.Find("Moon and Sun");
        stars = transform.Find("Stars");
        AdjustToCurrentTime();
    }

    private void Update()
    {
        _seconds += Time.deltaTime * GameTime.GetTimeScale() * 100;
        if (_checkMinute != GameTime.Minutes)
        {
            _checkMinute = GameTime.Minutes;
            _seconds = 0;
        }
        AdjustToCurrentTime();
    }
    private void AdjustToCurrentTime()
    {
        _currentTimeDegrees = (float)(GameTime.Hours * 60 + GameTime.Minutes + _seconds / 60) / _convertTimeToRotation;
        moonAndSun.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -_currentTimeDegrees);
        switch (_currentTimeDegrees)
        {
            case float n when n is > 315f and <= 360f or >= 0f and <= 75f: //полностью темно, 21:00 - 05:00
                StarsSetAlpha(stars, 1f);
                break;

            case float n when n is > 75f and <= 135f:   //рассвет, 05:00 - 09:00
                StarsSetAlpha(stars, 1f - (n - 75f) / 60f);
                break;

            case float n when n is > 135f and <= 255f:  //полностью светло, 09:00 - 17:00
                StarsSetAlpha(stars, 0f);
                break;

            case float n when n is > 255f and <= 315f:  //закат, 17:00 - 21:00 
                StarsSetAlpha(stars, (n - 255f) / 60f);
                break;
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
