using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventInTravelInfoPanel : MonoBehaviour
{
    public TMP_Text _infoText;
    private EventWindow _eventWindow;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        StartCoroutine(AppearAndDisappear());
    }

    public void Init(EventWindow eventWindow)
    {
        _eventWindow = eventWindow;
    }

    private IEnumerator AppearAndDisappear()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        
        for (int i = 0; i < 30; i++)
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
        _eventWindow.DestroyInfoPanel();
    }
}
