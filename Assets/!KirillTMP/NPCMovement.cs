using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


abstract public class NPCMovement : MonoBehaviour
{
    public bool DefaultViewDirectionIsRight = true;
    [SerializeField] protected ContactFilter2D _groundMask;
    [SerializeField] protected float _speed = 2f;
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
    [HideInInspector] public Animator Animator;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        _npc = GetComponent<Npc>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _groundMask.useLayerMask = true;
        LayerMask layerMask = new LayerMask();
        layerMask = 128; // 2^7
        _groundMask.layerMask = layerMask;
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
        Animator.SetTrigger("IDLE");
        WaitForSeconds waitForSeconds;
        if (quick)
            waitForSeconds = new(Random.Range(1, 1.5f));
        else
            waitForSeconds = new(Random.Range(_minIDLEDuration, _maxIDLEDuration));
        yield return waitForSeconds;
        _currentCoroutine = StartCoroutine(Move());
        Animator.SetTrigger("Move");
    }

    public void MakeNPCBusy()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
        
        _currentCoroutine = StartCoroutine(IDLEWhileBusy());
        TurnToPlayer();
    }
    
    private IEnumerator IDLEWhileBusy()
    {
        Animator.SetTrigger("IDLE");
        WaitForSeconds waitForSeconds = new WaitForSeconds(100f);
        while (true)
        {
            yield return waitForSeconds;
        }
    }

    public void NPCMakeFree()
    {
        StopCoroutine(_currentCoroutine);
        StartIDLE(true);
    }

    private void TurnToPlayer()
    {
        bool playerRighter = transform.position.x < Player.Instance.transform.position.x;
        if (DefaultViewDirectionIsRight)
        {
            if (playerRighter)
                _spriteRenderer.flipX = false;
            else
                _spriteRenderer.flipX = true;
        }
        else
        {
            if (playerRighter)
                _spriteRenderer.flipX = true;
            else
                _spriteRenderer.flipX = false;
        }
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
        if (MoveByNodeIsActive) 
            return;
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
            EnableNPC(false);
        }
    }
    
    public void OnCollisionWithNode(Node node)
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(node.MoveNPCByNode(this));
    }

    

    private void OnHourChangeWhenAtHome()
    {
        if (GameTime.Hours >= _npc.NpcData.StartWalkingTime && GameTime.Hours < _npc.NpcData.FinishWalkingTime)
        {
            GameTime.HourChanged -= OnHourChangeWhenAtHome;
            GameTime.HourChanged += OnHourChangeWhenNotAtHome;
            EnableNPC(true);
            StartIDLE(true);
        }
    }

    private void EnableNPC(bool enable)
    {
        _spriteRenderer.enabled = enable;
        enabled = enable;
        _collider.enabled = enable;
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

}
