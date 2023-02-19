using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepDurationPanel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _doneButton;
    [SerializeField] private Timeflow _timeflow;
    [SerializeField] private int _timeScaleWhenSleeping;

    private Player _player;

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

    private void Awake() //OnEnable גûחûגאועסÿ המ Start(), םמ ןמסכו Awake().
    {
        _player = Player.Instance;
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
