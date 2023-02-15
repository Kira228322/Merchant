using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NPCClickFunctional : ClickableIfPlayerNear
{
    [SerializeField] private GameObject _functionalWindow;
    private NPC _NPC;
    private Canvas _canvas;
    

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();    
        _NPC = GetComponent<NPC>();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        
        GameObject window = Instantiate(_functionalWindow, _canvas.transform);
        window.transform.position = Camera.main.WorldToScreenPoint(new Vector3((transform.position.x + _player.transform.position.x) / 2,
             transform.position.y + 3.5f));
        window.GetComponent<FunctionalWindow>().Init(_NPC);
    }
}
