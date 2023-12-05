using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour
{
    [SerializeField] private Image _currentTimeImage;
    [SerializeField] private Image _nextTimeImage;
    [SerializeField] private GameObject _detailedTimeObject;
    [SerializeField] private TMPro.TMP_Text _detailedTime;
    [SerializeField] private List<Sprite> _timeSprites = new();
    [SerializeField] private List<Sprite> _weatherSprites = new();
    private WeatherController _weatherController;
    private Coroutine _currentCoroutine;
    private Coroutine _sleepCoroutine;
    


    private void OnEnable()
    {
        _weatherController = FindObjectOfType<WeatherController>();
        GameTime.HourChanged += OnHourChanged;
        GameTime.TimeSkipped += OnTimeSkipped;
        _weatherController.WeatherStarted += OnWeatherIsActive;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
        GameTime.TimeSkipped -= OnTimeSkipped;
        _weatherController.WeatherStarted -= OnWeatherIsActive;
    }

    private void Start()
    {
        _currentTimeImage.sprite = _timeSprites[0];
        Refresh();
    }

    private void OnWeatherIsActive()
    {
        switch (GameTime.Hours)
        {
            case int n when n >= 5 && n <= 14:
                if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                    _nextTimeImage.sprite = _weatherSprites[3];
                else _nextTimeImage.sprite = _weatherSprites[0];
                break;
            case int n when n >= 15 && n <= 21:
                if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                    _nextTimeImage.sprite = _weatherSprites[4];
                else _nextTimeImage.sprite = _weatherSprites[1];
                break;
            case int n when n >= 22 || n <= 4:
                if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                    _nextTimeImage.sprite = _weatherSprites[5];
                else _nextTimeImage.sprite = _weatherSprites[2];
                break;
        }
        ChangeImage();
    }
    private void OnHourChanged()
    {
        Refresh();
    }
    private void OnTimeSkipped(int skippedDays, int skippedHours, int skippedMinutes)
    {
        Refresh();
    }

    public void Refresh()
    {
        bool isWeatherActive = GlobalEventHandler.Instance.IsEventActive<GlobalEvent_Weather>();
        if (isWeatherActive)
        {
            OnWeatherIsActive();
            return;
        }
        switch (GameTime.Hours)
        {
            case int n when n >= 5 && n <= 9:
                _nextTimeImage.sprite = _timeSprites[0];
                break;
            case int n when n >= 10 && n <= 14:
                _nextTimeImage.sprite = _timeSprites[1];
                break;
            case int n when n >= 15 && n <= 18:
                _nextTimeImage.sprite = _timeSprites[2];
                break;
            case int n when n >= 19 && n <= 21:
                _nextTimeImage.sprite = _timeSprites[3];
                break;
            case int n when n >= 22 || n <= 1:
                _nextTimeImage.sprite = _timeSprites[4];
                break;
            case int n when n >= 2 && n <= 4:
                _nextTimeImage.sprite = _timeSprites[5];
                break;
        }

        ChangeImage();
    }

    private void ChangeImage()
    {
        if (_nextTimeImage.sprite == _currentTimeImage.sprite)
            return;

        StartCoroutine(ChangeImageAnimation());
    }

    private IEnumerator ChangeImageAnimation()
    {
        WaitForSeconds waitForSeconds = new(0.02f);
        Color currentColor = _currentTimeImage.color;
        Color nextColor = _nextTimeImage.color;
        while (_currentTimeImage.color.a > 0)
        {
            currentColor.a -= 0.02f;
            nextColor.a += 0.02f;
            _currentTimeImage.color = currentColor;
            _nextTimeImage.color = nextColor;
            yield return waitForSeconds;
        }

        _currentTimeImage.sprite = _nextTimeImage.sprite;
        _currentTimeImage.color = nextColor; // альфа 1
        _nextTimeImage.color = currentColor; // альфа 0
    }
    
    public void OnClockClicked()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        _currentCoroutine = StartCoroutine(ShowDetailedTime());
    }
    private IEnumerator ShowDetailedTime()
    {
        WaitForSeconds waitForSeconds = new(0.1f);
        _detailedTimeObject.SetActive(true);
        for (int i = 0; i < 30; i++)
        {
            _detailedTime.text = $"День {GameTime.CurrentDay}\n{GameTime.Hours:D2}:{GameTime.Minutes:D2}";
            yield return waitForSeconds;
        }
        _detailedTimeObject.SetActive(false);
    }

    private IEnumerator ShowDetailedTimeWhileSleep()
    {
        WaitForSeconds waitForSeconds = new(0.1f);
        _detailedTimeObject.SetActive(true);
        while(true)
        {
            _detailedTime.text = $"День {GameTime.CurrentDay}\n{GameTime.Hours:D2}:{GameTime.Minutes:D2}";
            yield return waitForSeconds;
        }
    }

    public void OnOpenSleepPanel()
    {
        _sleepCoroutine = StartCoroutine(ShowDetailedTimeWhileSleep());
    }

    public void OnCloseSleepPanel()
    {
        if (_sleepCoroutine != null)
            StopCoroutine(_sleepCoroutine);
        _detailedTimeObject.SetActive(false);
        _sleepCoroutine = null;
    }

}
