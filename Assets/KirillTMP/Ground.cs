using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ground : MonoBehaviour, IPointerClickHandler
{
    private PlayerMover _playerMover;
    private float _speed;  
    private Coroutine _currentMove;
    
    private void Start()
    {
        _playerMover = FindObjectOfType<PlayerMover>();
        _speed = _playerMover.Speed;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 targetPosition = Camera.main.ScreenPointToRay(eventData.position).origin;
        targetPosition = new Vector3(targetPosition.x, _playerMover.transform.position.y);
        if (_currentMove != null)
        {
            StopCoroutine(_currentMove);
            _currentMove = null;
        }
        _currentMove = StartCoroutine(_playerMover.Move(_playerMover.transform.position ,targetPosition));
    }

    
}
