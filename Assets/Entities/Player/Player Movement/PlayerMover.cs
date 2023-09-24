using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMover : MonoBehaviour
{
    [FormerlySerializedAs("_backgroundController")] [SerializeField] public BackgroundController BackgroundController;
    [SerializeField] private ContactFilter2D _contactFilter2D;
    private float _speed = 3.9f;
    [HideInInspector]public float _currentSpeed;

    public float SpeedModifier = 0;

    [HideInInspector] public Rigidbody2D _rigidbody;
    [HideInInspector] public Collider2D _collider;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    private Coroutine _currentMove;

    private float _moveCD = 0.3f;
    private float _currentMoveCD;
    private Vector3 _finishTargetPos;
    [HideInInspector] public Vector3 _lastNodePos;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        BackgroundController.UpdateBackground(transform.position.x);
        ChangeCurrentSpeed();
    }

    public void DisableMove()
    {
        _animator.SetTrigger("IDLE");
        if (_currentMove != null)
        {
            StopCoroutine(_currentMove);
            _currentMove = null;
            _rigidbody.velocity = new Vector2(0,0);
        }

        enabled = false;
    }

    public void EnableMove()
    {
        enabled = true;
    }

    public void StartMove(Vector3 startPos,Vector3 targetPos)
    {
        if (enabled == false)
            return;
        
        if (_currentMoveCD == 0)
        {
            _currentMoveCD = _moveCD;
            StartCoroutine(ResetMoveCD());
            
            if (_currentMove != null)
            {
                StopCoroutine(_currentMove);
                _currentMove = null;
                _rigidbody.velocity = new Vector2(0,0);
            }
            else _animator.SetTrigger("Move");
            _currentMove = StartCoroutine(Move(startPos, targetPos));
        }
    }

    private IEnumerator ResetMoveCD()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_moveCD);
        yield return waitForSeconds;
        _currentMoveCD = 0;
    }

    public void ChangeCurrentSpeed()
    {
        _currentSpeed = _speed * (1 + SpeedModifier);
    }
    
    public void MoveByNode(Node node)
    {
        if (_currentMove != null)
        {
            StopCoroutine(_currentMove);
        }
        
        _currentMove = StartCoroutine(node.MovePlayerByNode(this));
    }

    public void MoveAfterNode()
    {
        if (_currentMove != null)
        {
            StopCoroutine(_currentMove);
        }
        // если точка задана на лестницу, то движение не продолжается
        if (_lastNodePos.x < transform.position.x && _lastNodePos.x + 0.5f >_finishTargetPos.x)
        {
            _animator.SetTrigger("IDLE");
            _currentMove = null;
            return;
        }
        if (_lastNodePos.x > transform.position.x && _lastNodePos.x - 0.5f < _finishTargetPos.x)
        {
            _animator.SetTrigger("IDLE");
            _currentMove = null;
            return;
        }
        // если нужно двигаться дальше
        
        StartMove(transform.position, _finishTargetPos);
    }
    
    public IEnumerator Move(Vector3 startPos,Vector3 targetPos)
    {
        RevertPlayer(targetPos.x > startPos.x);
        WaitForEndOfFrame forEndOfFrameUnit = new WaitForEndOfFrame();
        yield return forEndOfFrameUnit;
        _finishTargetPos = targetPos;
        Vector3 moveDirection;
        if (targetPos.x > startPos.x)
            moveDirection = Vector3.right;
        else
            moveDirection = Vector3.left;
        
        RaycastHit2D[] raycastHit2D = new RaycastHit2D[8];

        Vector2 castDirection = new Vector2(targetPos.x - startPos.x, 0.05f); //+0.05y,чтобы не коллизился с полом

        float distance;
        
        if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, 
                Math.Abs(targetPos.x - startPos.x) ) != 0)
        {
            distance = raycastHit2D[0].distance;
        }
        else
        {
            distance = Math.Abs(targetPos.x - startPos.x);
        }
        
        float travelledDistance = 0f;
        while (travelledDistance < distance)
        {
            travelledDistance += _currentSpeed * Time.deltaTime;
            transform.position += _currentSpeed * Time.deltaTime * moveDirection;
            BackgroundController.UpdateBackground(transform.position.x);
            yield return forEndOfFrameUnit;
        }
        _animator.SetTrigger("IDLE");
        _currentMove = null;
    }

    private void RevertPlayer(bool rightMove)
    {
        if (rightMove)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;
    }
    
}
