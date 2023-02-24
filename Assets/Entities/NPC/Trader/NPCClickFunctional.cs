using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NPCClickFunctional : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _functionalWindow;
    private NPC _NPC;
    private Canvas _canvas;
    
    private float _distanceToUse = 3.5f;
    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();    
        _NPC = GetComponent<NPC>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;
        
        GameObject window = Instantiate(_functionalWindow, _canvas.transform);
        window.transform.position = Camera.main.WorldToScreenPoint(new Vector3((transform.position.x + Player.Instance.transform.position.x) / 2,
             transform.position.y + 3.5f));
        window.GetComponent<FunctionalWindow>().Init(_NPC);
    }
}
