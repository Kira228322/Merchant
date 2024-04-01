using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SleepDurationPanel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _doneButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Image _blackScreen;

    private int _timeScaleWhenSleeping = 45;
    private Player _player;
    private Timeflow _timeflow;
    private bool _animationFinished = true;
    private Coroutine _currentCoroutine;
    private void OnEnable()
    {
        _player.Needs.FinishedSleeping += OnFinishedSleeping;
        _player.Needs.SleptOneHourEvent += OnSleptOneHour;
    }
    private void OnDisable()
    {
        _player.Needs.FinishedSleeping -= OnFinishedSleeping;
        _player.Needs.SleptOneHourEvent -= OnSleptOneHour;
        Player.Instance.PlayerMover.EnableMove();
        GameManager.Instance.UIClock.OnCloseSleepPanel();
    }

    private void Awake()
    {
        _player = Player.Instance;
        _player.GetComponent<Rigidbody2D>().simulated = false;
        _timeflow = FindObjectOfType<Timeflow>();
        GameManager.Instance.UIClock.OnOpenSleepPanel();
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
        _player.HidePlayer(false);
        Player.Instance.PlayerMover.EnableMove();
        if (!_animationFinished)
            StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(FadeOutBlackScreen());
        GameManager.Instance.SaveGame();
    }

    public void OnDoneButtonPressed()
    {
        _player.Needs.StartSleeping((int)_slider.value);
        _timeflow.TimeScale = _timeScaleWhenSleeping;
        _slider.interactable = false;
        _player.HidePlayer(true);
        if (!_animationFinished)
            StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(FadeInBlackScreen());
    }

    private IEnumerator FadeInBlackScreen()
    {
        _animationFinished = false;
        Color color = _blackScreen.color;
        WaitForSeconds waitForSeconds = new(0.02f);
        color.a = 0;
        for (int i = 0; i < 60; i++)
        {
            color.a += 0.01f;
            _blackScreen.color = color;
            yield return waitForSeconds;
        }

        _animationFinished = true;
    }

    private IEnumerator FadeOutBlackScreen()
    {
        _animationFinished = false;
        Color color = _blackScreen.color;
        WaitForSeconds waitForSeconds = new(0.02f);
        for (int i = 0; i < 60; i++)
        {
            color.a -= 0.01f;
            _blackScreen.color = color;
            yield return waitForSeconds;
        }
        _animationFinished = true;
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
