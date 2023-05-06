using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCMovementAroudPoint : NPCMovement
{
    // Не учитывает наличие стен в радиусе обхода. стен быть не должно! 
    
    // Думал задавать _pointPosition изначально через инспектор, но потом понял, что удобнее просто ставить
    // торговца на сцене в ту точку, вокруг которой он и будет гулять 
    
    private Vector2 _pointPosition;
    public Vector2 PointPosition => _pointPosition;
    [SerializeField] private float _radius;
    
    protected override void Awake()
    {
        base.Awake();
        _pointPosition = transform.position;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(Move());
            Animator.SetTrigger("Move");
        }
    }
    
    private void ChooseMoveRangeAndDirection()
    {
        MoveDistanceAndDirection = Random.Range(_radius * 0.75f, _radius * 1.5f);
        if (Random.Range(0, 2) == 0)
            MoveDistanceAndDirection = -MoveDistanceAndDirection;
    }
    
    protected override IEnumerator Move()
    {
        ChooseMoveRangeAndDirection();
        StartPosition = transform.position;
        Vector3 targetPosition = new(StartPosition.x + MoveDistanceAndDirection, StartPosition.y);
        
        if (targetPosition.x > _pointPosition.x + _radius)
            targetPosition.x = _pointPosition.x + _radius;
        else if (targetPosition.x < _pointPosition.x - _radius)
            targetPosition.x = _pointPosition.x - _radius;
        
        RevertViewDirection(targetPosition.x - StartPosition.x > 0);
        
        WaitForFixedUpdate waitForFixedUpdate = new();
        float countOfFrames = Math.Abs((targetPosition.x - StartPosition.x) / (_speed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            transform.position = Vector3.Lerp(StartPosition, targetPosition, i/countOfFrames);
            yield return  waitForFixedUpdate;
        }
        
        transform.position = targetPosition;
        StartIDLE(false);
    }
}
