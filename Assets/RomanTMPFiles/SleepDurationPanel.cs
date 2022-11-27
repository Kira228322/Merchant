using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepDurationPanel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _doneButton;
    private Player _player;

    private void OnEnable()
    {
        GameTime.HourChanged += OnHourChanged;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void OnHourChanged()
    {
        if (_player.IsSleeping)
        {
            _slider.value--;
        }
        else if (_doneButton.interactable == false) _doneButton.interactable = true;
    }

    public void OnDoneButtonPressed()
    {
        _player.Sleep((int)_slider.value);
    }
}
