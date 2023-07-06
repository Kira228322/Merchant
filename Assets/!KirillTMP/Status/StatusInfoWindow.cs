using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfoWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _duration;
    [SerializeField] private Slider _slider;
    private CanvasGroup _canvasGroup;

    public void Init(string name, string description, float currentDuration, float duration)
    {
        StatusManager.Instance.CurrentStatusInfoWindow = gameObject;
        _name.text = name;
        _description.text = description;
        _duration.text = $"Длительность эффекта: {Math.Round(currentDuration,1)}/{(int)duration} ч";
        _slider.maxValue = duration;
        _slider.value = currentDuration;
        StartCoroutine(FadeOut());
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator FadeOut()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(5);
        yield return waitForSeconds;
        waitForSeconds = new WaitForSeconds(0.02f);
        for (int i = 0; i < 50; i++)
        {
            _canvasGroup.alpha -= 0.02f;
            yield return waitForSeconds;
        }
        StatusManager.Instance.CurrentStatusInfoWindow = null;
        Destroy(gameObject);
    }
}
