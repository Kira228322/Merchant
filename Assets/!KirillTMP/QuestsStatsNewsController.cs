using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsStatsNewsController : MonoBehaviour
{
    [HideInInspector]public GameObject CurrentActivePanel;
    [HideInInspector]public QuestsStatsNewsButton CurrentActiveButton;

    [SerializeField] private GameObject _firstPanel;
    [SerializeField] private QuestsStatsNewsButton _firstButton;

    public void OnQSNbuttonClick()
    {
        gameObject.SetActive(true);
        
        if (CurrentActivePanel != null)
            CurrentActivePanel.SetActive(false);
        CurrentActivePanel = _firstPanel;
        CurrentActivePanel.SetActive(true);
        
        if (CurrentActiveButton != null)
            CurrentActiveButton.OnDeselectButton();
        CurrentActiveButton = _firstButton;
        CurrentActiveButton.OnSelectButton();
    }
}
