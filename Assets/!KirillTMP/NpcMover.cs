using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcMover : MonoBehaviour
{
    [SerializeField] private LayerMask _groundAndNodeMask;
    private float _speed = 2.8f;
    private float _minMoveDistance = 2f;
    private float _maxMoveDistance = 6f;
    private float _minIDLEDuration = 2f; // TODO значения для теста. Потом сбалансить
    private float _maxIDLEDuration = 3f;
    private float _moveDistanceAndDirection;
    private Vector3 _startPosition;
    private Coroutine _currentCoroutine;
    
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(Move());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 ) // Node
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(MoveByNode(col.gameObject.GetComponent<Node>().AnotherNode.position));
        }
    }

    private void ChooseMoveRangeAndDirection()
    {
        _moveDistanceAndDirection = Random.Range(_minMoveDistance, _maxMoveDistance);
        if (Random.Range(0, 2) == 0)
            _moveDistanceAndDirection = -_moveDistanceAndDirection;

        List<RaycastHit2D> raycastHits2D = new List<RaycastHit2D>();
        _rigidbody.Cast(new Vector2(_moveDistanceAndDirection, 0.1f), raycastHits2D, _groundAndNodeMask);

        if (raycastHits2D.Count > 0)
        {
            _moveDistanceAndDirection = (raycastHits2D[0].distance + 0.01f) * Math.Sign(_moveDistanceAndDirection);
        }
    }
    
    private IEnumerator Move()
    {
        ChooseMoveRangeAndDirection();
        _startPosition = transform.position;
        Vector3 targetPosition = new Vector3(_startPosition.x + _moveDistanceAndDirection, _startPosition.y);
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        float countOfFrames = Math.Abs(_moveDistanceAndDirection / (_speed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            transform.position = Vector3.Lerp(_startPosition, targetPosition, i/countOfFrames);
            yield return  waitForFixedUpdate;
        }

        transform.position = targetPosition;
        _currentCoroutine = StartCoroutine(IDLE());
    }

    private IEnumerator MoveByNode(Vector3 targetPosition)
    {
        _collider.enabled = false;
        _startPosition = transform.position;
        _moveDistanceAndDirection = Random.Range(0.8f, 1.2f) * Math.Sign(targetPosition.x - _startPosition.x); 
        // Random.Range(0.8f, 1.2f) - кастыль, надежда, что после ступенек не будет никакого барьера (По логике
        // если мы ставим ступеньки, то они куда-то обязательно должны вести, и сразу же тупика быть не должно)
        // Если сильно надо будет -- переделаю, так экономнее выходит для производительности
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        
        float countOfFrames = (targetPosition-_startPosition).magnitude / (_speed/1.33f * Time.fixedDeltaTime);
        
        for (float i = 0; i < countOfFrames; i++)
        {
            transform.position = Vector3.Lerp(_startPosition, targetPosition, i/countOfFrames);
            yield return  waitForFixedUpdate; // по ступенькам поднимаемся
        }
        
        transform.position = targetPosition;
        
        _startPosition = transform.position;
        targetPosition = new Vector3(_startPosition.x + _moveDistanceAndDirection, _startPosition.y);

        countOfFrames = Math.Abs(_moveDistanceAndDirection / (_speed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            transform.position = Vector3.Lerp(_startPosition, targetPosition, i/countOfFrames);
            yield return  waitForFixedUpdate; // а потом идем дальше
        }

        transform.position = targetPosition;
        _collider.enabled = true;
        _currentCoroutine = StartCoroutine(IDLE());
    }

    private IEnumerator IDLE()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(Random.Range(_minIDLEDuration, _maxIDLEDuration));
        yield return waitForSeconds;
        _currentCoroutine = null;
    }
}
