using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCMovementByScene : NPCMovement
{
    private float _minMoveDistance = 3f; 
    private float _maxMoveDistance = 12f; 
    
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
        MoveDistanceAndDirection = Random.Range(_minMoveDistance, _maxMoveDistance);
        if (Random.Range(0, 2) == 0)
            MoveDistanceAndDirection = -MoveDistanceAndDirection;

        List<RaycastHit2D> raycastHits2D = new();
        _rigidbody.Cast(new Vector2(MoveDistanceAndDirection, 0.1f), _groundMask, raycastHits2D, Math.Abs(MoveDistanceAndDirection));
        
        if (raycastHits2D.Count > 0)
        {
            MoveDistanceAndDirection = (raycastHits2D[0].distance - 0.01f) * Math.Sign(MoveDistanceAndDirection);
        }
        RevertViewDirection(MoveDistanceAndDirection > 0);
    }
    
    protected override IEnumerator Move()
    {
        ChooseMoveRangeAndDirection();
        StartPosition = transform.position;
        Vector3 targetPosition = new(StartPosition.x + MoveDistanceAndDirection, StartPosition.y);
        WaitForFixedUpdate waitForFixedUpdate = new();
        
        float countOfFrames = Math.Abs(MoveDistanceAndDirection / (_speed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            transform.position = Vector3.Lerp(StartPosition, targetPosition, i/countOfFrames);
            yield return  waitForFixedUpdate;
        }
        
        transform.position = targetPosition;
        StartIDLE(false);
    }
    
}
