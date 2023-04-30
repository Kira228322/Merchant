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
    [SerializeField] private List<Sprite> _timeSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _weatherSprites = new List<Sprite>();
    private WeatherController _weatherController;
    private Coroutine _currentCoroutine;
    


    private void OnEnable()
    {
        _weatherController = FindObjectOfType<WeatherController>();
        GameTime.HourChanged += OnHourChanged;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
    }

    private void Start()
    {
        _currentTimeImage.sprite = _timeSprites[0];
    }

    private void OnHourChanged()
    {
        switch (GameTime.Hours)
        {
            case int n when n >= 5 && n <= 9:
                if (0 == 1) // TODO если идет дождь
                    if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                        _nextTimeImage.sprite = _weatherSprites[3];
                    else
                        _nextTimeImage.sprite = _weatherSprites[0];
                
                _nextTimeImage.sprite = _timeSprites[0];
                break;
            case int n when n >= 10 && n <= 14:
                if (0 == 1) // TODO если идет дождь
                    if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                        _nextTimeImage.sprite = _weatherSprites[3];
                    else
                        _nextTimeImage.sprite = _weatherSprites[0];
                
                _nextTimeImage.sprite = _timeSprites[1];
                 break;
            case int n when n >= 15 && n <= 18:
                if (0 == 1) // TODO если идет дождь
                    if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                        _nextTimeImage.sprite = _weatherSprites[4];
                    else
                        _nextTimeImage.sprite = _weatherSprites[1];
                
                _nextTimeImage.sprite = _timeSprites[2];
                break;
            case int n when n >= 19 && n <= 21:
                if (0 == 1) // TODO если идет дождь
                    if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                        _nextTimeImage.sprite = _weatherSprites[4];
                    else
                        _nextTimeImage.sprite = _weatherSprites[1];
                
                _nextTimeImage.sprite = _timeSprites[3];
                break;
            case int n when n >= 22 || n <= 1:
                if (0 == 1) // TODO если идет дождь
                    if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                        _nextTimeImage.sprite = _weatherSprites[5];
                    else
                        _nextTimeImage.sprite = _weatherSprites[2];
                
                _nextTimeImage.sprite = _timeSprites[4];
                break;
            case int n when n >= 2 && n <= 4:
                if (0 == 1) // TODO если идет дождь
                    if ((int)_weatherController.WeatherStrength == 2) // сильный дождь
                        _nextTimeImage.sprite = _weatherSprites[5];
                    else
                        _nextTimeImage.sprite = _weatherSprites[2];
                
                _nextTimeImage.sprite = _timeSprites[5];
                break;
        }
        
        if (_nextTimeImage.sprite == _currentTimeImage.sprite)
            return;

        StartCoroutine(ChangeImageAnimation());
    }

    private IEnumerator ChangeImageAnimation()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
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
        WaitForSeconds waitForSeconds = new(2.5f);
        _detailedTimeObject.gameObject.SetActive(true);
        _detailedTime.text = $"День {GameTime.CurrentDay}\n{GameTime.Hours:D2}:{GameTime.Minutes:D2}";
        yield return waitForSeconds;
        _detailedTimeObject.gameObject.SetActive(false);
    }

}
