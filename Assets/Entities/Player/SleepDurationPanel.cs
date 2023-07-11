using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SleepDurationPanel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _doneButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Image _blackScreen;
    private int _timeScaleWhenSleeping = 30;
    private Player _player;
    private Timeflow _timeflow;

    private void OnEnable()
    {
        _player.Needs.FinishedSleeping += OnFinishedSleeping;
        _player.Needs.SleptOneHourEvent += OnSleptOneHour;
    }
    private void OnDisable()
    {
        _player.Needs.FinishedSleeping -= OnFinishedSleeping;
        _player.Needs.SleptOneHourEvent -= OnSleptOneHour;
    }

    private void Awake()
    {
        _player = Player.Instance;
        _player.GetComponent<Rigidbody2D>().simulated = false;
        _timeflow = FindObjectOfType<Timeflow>();
    }

    private void OnSleptOneHour()
    {
        _slider.value--;
    }
    private void OnFinishedSleeping()
    {
        _timeflow.TimeScale = 1;
        _doneButton.interactable = true;
        _closeButton.interactable = true;
        _slider.interactable = true;
        GameManager.Instance.ButtonsBlock.SetActive(true);
        _player.HidePlayer(false);
        StartCoroutine(FadeOutBlackScreen());
    }

    public void OnDoneButtonPressed()
    {
        _player.Needs.StartSleeping((int)_slider.value);
        _timeflow.TimeScale = _timeScaleWhenSleeping;
        _slider.interactable = false;
        GameManager.Instance.ButtonsBlock.SetActive(false);
        _player.HidePlayer(true);
        StartCoroutine(FadeInBlackScreen());
    }

    private IEnumerator FadeInBlackScreen()
    {
        Color color = _blackScreen.color;
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        for (int i = 0; i < 60; i++)
        {
            color.a += 0.01f;
            _blackScreen.color = color;
            yield return waitForSeconds;
        }
    } 
    
    private IEnumerator FadeOutBlackScreen()
    {
        Color color = _blackScreen.color;
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        for (int i = 0; i < 60; i++)
        {
            color.a -= 0.01f;
            _blackScreen.color = color;
            yield return waitForSeconds;
        }
    }

    public void OnExitButtonClick()
    {
        _player.GetComponent<Rigidbody2D>().simulated = true;
        Destroy(gameObject);
    }

    public void OnValueChanged()
    {
        if (Player.Instance.Needs.IsSleeping)
            return;
        
        if (_slider.value == 0)
            _doneButton.interactable = false;
        else
            _doneButton.interactable = true;
    }
}
