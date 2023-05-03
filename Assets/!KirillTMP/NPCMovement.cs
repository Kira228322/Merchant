using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


abstract public class NPCMovement : MonoBehaviour
{
    [SerializeField] protected ContactFilter2D _groundMask;
    [SerializeField] protected float _speed = 2.8f;
    public float Speed => _speed;
    protected float _minIDLEDuration = 2f; // 6 // TODO значения для теста. Потом сбалансить
    protected float _maxIDLEDuration = 3f; // 20
    [HideInInspector] public float MoveDistanceAndDirection;
    [HideInInspector] public Vector3 StartPosition;
    protected Coroutine _currentCoroutine;
    
    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;
    public Collider2D Collider => _collider;
    public Rigidbody2D Rigidbody => _rigidbody;
    [HideInInspector] public bool MoveByNodeIsActive = false;
    private bool _isGoingToHome = false;
    private Vector3 _home;
    private Npc _npc;
    private SpriteRenderer _spriteRenderer;

    protected virtual void Awake()
    {
        _npc = GetComponent<Npc>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        ChooseHome();
    }

    protected virtual void OnEnable()
    {
        GameTime.HourChanged += OnHourChangeWhenNotAtHome;
    }

    protected virtual void OnDisable()
    {
        GameTime.HourChanged -= OnHourChangeWhenNotAtHome;
    }

    public void StartIDLE(bool quick)
    {
        if (quick)
            _currentCoroutine = StartCoroutine(IDLE(true));
        else
            _currentCoroutine = StartCoroutine(IDLE(false));
        
    }
    private IEnumerator IDLE(bool quick)
    {
        WaitForSeconds waitForSeconds;
        if (quick)
            waitForSeconds = new(Random.Range(1, 1.5f));
        else
            waitForSeconds = new(Random.Range(_minIDLEDuration, _maxIDLEDuration));
        yield return waitForSeconds;
        _currentCoroutine = StartCoroutine(Move());
    }

    protected abstract IEnumerator Move();
    
    private void ChooseHome()
    {
        Home[] homes = FindObjectsOfType<Home>();
        _home = homes[Random.Range(0, homes.Length)].DoorTransform.position;
    }

    private void OnHourChangeWhenNotAtHome()
    {
        if (GameTime.Hours >= _npc.NpcData.FinishWalkingTime)
        {
            GameTime.MinuteChanged += OnMinuteChange;
            GameTime.HourChanged -= OnHourChangeWhenNotAtHome;
            if (!MoveByNodeIsActive)
            {
                if (_currentCoroutine != null)
                    StopCoroutine(_currentCoroutine);
                _currentCoroutine = StartCoroutine(MoveDirectlyAtHome(_home.x));
                
            }
        }
    }

    private void OnMinuteChange()
    {
        if (!MoveByNodeIsActive)
            if (_isGoingToHome)
            {
                if (_currentCoroutine != null)
                    StopCoroutine(_currentCoroutine);
                _currentCoroutine = StartCoroutine(MoveDirectlyAtHome(_home.x));
            }
            else
            {
                GameTime.MinuteChanged -= OnMinuteChange;
                GameTime.HourChanged += OnHourChangeWhenAtHome;
                _spriteRenderer.enabled = false;
                enabled = false;
            }
    }
    
    public void OnCollisionWithNode(Node node)
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(node.MoveByNode(this));
    }

    private void OnHourChangeWhenAtHome()
    {
        if (GameTime.Hours >= _npc.NpcData.StartWalkingTime && GameTime.Hours < _npc.NpcData.FinishWalkingTime)
        {
            GameTime.HourChanged -= OnHourChangeWhenAtHome;
            GameTime.HourChanged += OnHourChangeWhenNotAtHome;
            _spriteRenderer.enabled = true;
            enabled = true;
            StartIDLE(true);
        }
    }
    
    private IEnumerator MoveDirectlyAtHome(float targetPosX)
    {
        _isGoingToHome = true;
        MoveDistanceAndDirection = targetPosX - transform.position.x;
        StartPosition = transform.position;
        Vector3 targetPosition = new(targetPosX, StartPosition.y);
        WaitForFixedUpdate waitForFixedUpdate = new();
        
        float countOfFrames = Math.Abs(MoveDistanceAndDirection / (_speed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            transform.position = Vector3.Lerp(StartPosition, targetPosition, i/countOfFrames);
            yield return  waitForFixedUpdate;
        }
        
        transform.position = targetPosition;
        _isGoingToHome = false;
    }


}
