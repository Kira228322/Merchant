using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class PlayerMovementHandler : MonoBehaviour
{
    public float Speed;
    private float _currentMoveCooldown;

    private bool _isAbleToMove = true; //Может ли получить новый приказ о движении в данный момент (не может, когда двигается по лестнице)

    private readonly float _moveCooldown = 0.3f;
    private readonly string _movementNodeTag = "MovementNode";

    private Coroutine _currentMoveCoroutine;

    private Vector2 _targetPosition;

    public void StartMove(Vector2 startPosition, Vector2 targetPosition)
    {
        if (!_isAbleToMove) return;
        if (_currentMoveCooldown == 0)
        {
            _currentMoveCooldown = _moveCooldown;
            StartCoroutine(ResetMoveCooldown());


            if (_currentMoveCoroutine != null)
            {
                StopCoroutine(_currentMoveCoroutine);
                //camera coroutine
                _currentMoveCoroutine = null;

            }

            _targetPosition = targetPosition;
            _currentMoveCoroutine = StartCoroutine(Move(startPosition, _targetPosition));
            //camera coroutine move
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_movementNodeTag))
        {
            Vector2 startPosition = collision.gameObject.transform.position;
            Vector2 furthestNodePosition;
            //Определить, какая точка ближе к игроку, чтобы найти ту, к которой нужно идти
            //(у collision всего 2 ребенка, 2 узла, один наверху лестницы, один внизу)
            if (Vector2.Distance(transform.position, collision.transform.GetChild(0).position) <
                Vector2.Distance(transform.position, collision.transform.GetChild(1).position))
            {
                furthestNodePosition = collision.transform.GetChild(1).position;
            }
            else
                furthestNodePosition = collision.transform.GetChild(0).position;

            StopCoroutine(_currentMoveCoroutine);
            _currentMoveCoroutine = StartCoroutine(MoveToOtherNodeAndContinueMovingToATarget(startPosition, furthestNodePosition, _targetPosition));
        }
    }
    private IEnumerator ResetMoveCooldown()
    {
        yield return new WaitForSeconds(_moveCooldown);
        _currentMoveCooldown = 0;
    }
    private IEnumerator MoveToOtherNodeAndContinueMovingToATarget(Vector2 startPosition, Vector2 nodePosition, Vector2 targetPosition)
    {
        _isAbleToMove = false;
        yield return Move(startPosition, nodePosition);
        _isAbleToMove = true;
        if (Vector2.Angle(startPosition, targetPosition) < Vector2.Angle(startPosition, nodePosition)) 
            //Если таргет оказался в противоположном направлении значит игрок еблан и кликнул на лестницу. Двигаться никуда не надо
        { 
            
            Vector2 newTargetPosition = new(targetPosition.x, transform.position.y); //Изменилась y, спустился или поднялся по лестнице
            yield return Move(nodePosition, newTargetPosition);
        }
    }

    public IEnumerator Move(Vector2 startPosition, Vector2 endPosition)
    {
        float step = (Speed / (startPosition - endPosition).magnitude) * Time.deltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return new WaitForFixedUpdate();
        }
        transform.position = endPosition;

    }
}
