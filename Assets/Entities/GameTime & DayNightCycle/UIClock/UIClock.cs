using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _detailedTime;
    [SerializeField] private Transform _clockRotatingPart;

    private Coroutine _currentCoroutine;


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
        _clockRotatingPart.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -15 * GameTime.Hours);
    }

    private void OnHourChanged()
    {
        _clockRotatingPart.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -15 * GameTime.Hours);
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
        _detailedTime.gameObject.SetActive(true);
        _detailedTime.text = $"Δενό {GameTime.CurrentDay}\n{GameTime.Hours:D2}:{GameTime.Minutes:D2}";
        yield return waitForSeconds;
        _detailedTime.gameObject.SetActive(false);
    }

}
