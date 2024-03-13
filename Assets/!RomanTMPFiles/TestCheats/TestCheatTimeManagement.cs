using TMPro;
using UnityEngine;

public class TestCheatTimeManagement : MonoBehaviour
{
    [SerializeField] private TMP_InputField _timescaleInputField;

    [SerializeField] private TMP_InputField _dayInputField;
    [SerializeField] private TMP_InputField _hourInputField;
    [SerializeField] private TMP_InputField _minuteInputField;

    [SerializeField] private TMP_Text _currentTime;

    public void SetTimescale()
    {
        FindObjectOfType<Timeflow>().TimeScale = float.Parse(_timescaleInputField.text);
    }
    private void OnEnable()
    {
        GameTime.MinuteChanged += OnMinuteChanged;
    }
    private void OnDisable()
    {
        GameTime.MinuteChanged -= OnMinuteChanged;
    }

    private void OnMinuteChanged()
    {
        _currentTime.text = $"День: {GameTime.CurrentDay}, Час: {GameTime.Hours}, Минута: {GameTime.Minutes}";
    }

    public void SkipTime()
    {
        int day = int.Parse(_dayInputField.text);
        int hour = int.Parse(_hourInputField.text);
        int minute = int.Parse(_minuteInputField.text);

        GameTime.TimeSkip(day, hour, minute);
    }
    public void CheckDayInputField()
    {
        if (int.TryParse(_dayInputField.text, out int result))
        {
            if (result < 0)
                _dayInputField.text = "0";
        }
        else _dayInputField.text = "0";
    }
    public void CheckHourInputField()
    {
        if (int.TryParse(_hourInputField.text, out int parsed))
        {
            if (parsed < 0)
                _hourInputField.text = "0";
            if (parsed >= 24)
                _hourInputField.text = "23";
        }
        else _hourInputField.text = "0";
    }
    public void CheckMinuteInputField()
    {
        if (int.TryParse(_minuteInputField.text, out int parsed))
        {
            if (parsed < 0)
                _minuteInputField.text = "0";
            if (parsed >= 60)
                _minuteInputField.text = "59";
        }
        else _minuteInputField.text = "0";
    }

}
