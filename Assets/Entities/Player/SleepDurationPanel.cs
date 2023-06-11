using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SleepDurationPanel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _doneButton;
    [SerializeField] private int _timeScaleWhenSleeping;

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
    }

    public void OnDoneButtonPressed()
    {
        _player.Needs.StartSleeping((int)_slider.value);
        _timeflow.TimeScale = _timeScaleWhenSleeping;
    }
}
