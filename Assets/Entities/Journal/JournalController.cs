using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour
{
    [HideInInspector]public GameObject CurrentActivePanel;
    [HideInInspector]public JournalButton CurrentActiveButton;

    [SerializeField] private GameObject _firstPanel;
    [SerializeField] private JournalButton _firstButton;

    public void OnJournalButtonClick()
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
