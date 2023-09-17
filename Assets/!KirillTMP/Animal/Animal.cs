using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Animal : MonoBehaviour
{
    private float _minMoveDistance = 2f; 
    private float _maxMoveDistance = 9f; 

    private float _IDLEDuration = 4.5f;
    private float _AFKDuration = 15f;
    [SerializeField] protected float _speed = 2f;
    
    public bool DefaultViewDirectionIsRight = true;
    [HideInInspector] public float MoveDistanceAndDirection;
    
    [SerializeField] protected ContactFilter2D _contactMask;
    
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    [HideInInspector] public Animator Animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Move());
    }

    public void RevertViewDirection(bool moveDirectionIsRight)
    {
        if (moveDirectionIsRight)
        {
            if (DefaultViewDirectionIsRight)
                _spriteRenderer.flipX = false;
            else _spriteRenderer.flipX = true;
        }
        else if (DefaultViewDirectionIsRight)
            _spriteRenderer.flipX = true;
        else _spriteRenderer.flipX = false;
    }

    private void ChooseMoveRangeAndDirection()
    {
        MoveDistanceAndDirection = Random.Range(_minMoveDistance, _maxMoveDistance);
        if (Random.Range(0, 2) == 0)
            MoveDistanceAndDirection = -MoveDistanceAndDirection;

        List<RaycastHit2D> raycastHits2D = new();
        _rigidbody.Cast(new Vector2(MoveDistanceAndDirection, 0.1f), _contactMask, raycastHits2D, Math.Abs(MoveDistanceAndDirection));
        
        if (raycastHits2D.Count > 0)
        {
            MoveDistanceAndDirection = (raycastHits2D[0].distance - 0.01f) * Math.Sign(MoveDistanceAndDirection);
        }
        RevertViewDirection(MoveDistanceAndDirection > 0);
    }
    
    private IEnumerator Move()
    {
        ChooseMoveRangeAndDirection();
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new(startPosition.x + MoveDistanceAndDirection, startPosition.y);
        WaitForFixedUpdate waitForFixedUpdate = new();
        
        float countOfFrames = Math.Abs(MoveDistanceAndDirection / (_speed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, i/countOfFrames);
            yield return  waitForFixedUpdate;
        }
        
        transform.position = targetPosition;
        StartCoroutine(IDLEorAFK());
    }

    private IEnumerator IDLEorAFK()
    {
        WaitForSeconds waitForSeconds;
        if (Random.Range(0, 2) == 0)
        {
            Animator.SetTrigger("IDLE");
            waitForSeconds = new WaitForSeconds(Random.Range(_IDLEDuration * 0.75f, _IDLEDuration * 1.25f));
            yield return waitForSeconds;
        }
        else
        {
            Animator.SetTrigger("AFK");
            waitForSeconds = new WaitForSeconds(Random.Range(_AFKDuration * 0.75f, _AFKDuration * 1.25f));
            yield return waitForSeconds;
        }
        Animator.SetTrigger("Move");
        StartCoroutine(Move());
    }
}
