using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepDurationPanel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void OnDoneButtonPressed()
    {
        _player.Sleep((int)_slider.value);
    }
}
