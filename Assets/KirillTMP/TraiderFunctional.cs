using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Merchant))]
public class TraiderFunctional : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _functionalWindow;
    private Merchant _merchant;
    private Canvas _canvas;

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        _merchant = GetComponent<Merchant>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO проверить рядом ли находится плеер
        
        GameObject window = Instantiate(_functionalWindow, _canvas.transform);
        RectTransform rectTransform = window.GetComponent<RectTransform>();
        rectTransform.position = Input.mousePosition;
        window.GetComponent<FunctionalWindow>().Init(_merchant);
    }
}
