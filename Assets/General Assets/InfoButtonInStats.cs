using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButtonInStats : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = _panel.GetComponent<CanvasGroup>();
    }

    public void OnButtonClick()
    {
        if (_panel.activeSelf)
        {
            StopAllCoroutines();
            _panel.SetActive(false);
        }
        else
        {
            _panel.SetActive(true);
            StartCoroutine(AppearAndDisappear());
        }
    }
    
    private IEnumerator AppearAndDisappear()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        
        _canvasGroup.alpha = 0.066f;
        for (int i = 0; i < 28; i++)
        {
            _canvasGroup.alpha += 0.033f;
            yield return waitForSeconds;
        }

        _canvasGroup.alpha = 1f;
        waitForSeconds = new WaitForSeconds(5f);
        yield return waitForSeconds;
        
        waitForSeconds = new WaitForSeconds(0.02f);
        for (int i = 0; i < 40; i++)
        {
            _canvasGroup.alpha -= 0.025f;
            yield return waitForSeconds;
        }

        _canvasGroup.alpha = 0;
        _panel.SetActive(false);
    }
}
