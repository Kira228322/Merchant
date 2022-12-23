using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Trader))]
public class TraiderFunctional : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _functionalWindow;
    private Trader _trader;
    private Canvas _canvas;
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _canvas = FindObjectOfType<Canvas>();
        _trader = GetComponent<Trader>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - _player.transform.position).magnitude > 3.3f)
            return;
        
        GameObject window = Instantiate(_functionalWindow, _canvas.transform);
        window.transform.position = Camera.main.WorldToScreenPoint(new Vector3((transform.position.x + _player.transform.position.x) / 2,
             transform.position.y + 3.5f));
        window.GetComponent<FunctionalWindow>().Init(_trader);
    }
}
