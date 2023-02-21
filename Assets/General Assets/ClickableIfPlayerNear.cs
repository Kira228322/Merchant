using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ClickableIfPlayerNear : MonoBehaviour, IPointerClickHandler
{
    
    protected Player _player;
    
    private void Start()
    {
        _player = Player.Instance;
    }

    public  virtual void OnPointerClick(PointerEventData eventData)
    {
        _player = Player.Instance;
        if ((transform.position - _player.transform.position).magnitude > 3.3f)
            return;
    }
}
