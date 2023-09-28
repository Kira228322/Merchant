using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


abstract public class NPCMovement : MonoBehaviour
{
    public bool DefaultViewDirectionIsRight = true;
    [SerializeField] protected ContactFilter2D _groundMask;
    [SerializeField] protected float _speed = 2f;
    public float Speed => _speed;
    protected float _minIDLEDuration = 5f; 
    protected float _maxIDLEDuration = 15f; 
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
    private NpcClickFunctional _clickFunctional;
    private bool _currentStateIsIDLE; 

    protected virtual void Awake()
    {
        _clickFunctional = GetComponent<NpcClickFunctional>();
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
        OnHourChangeWhenNotAtHome();
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
        _currentStateIsIDLE = true;
    }
    private IEnumerator IDLE(bool quick)
    {
        Animator.SetTrigger("IDLE");
        WaitForSeconds waitForSeconds;
        if (quick)
            waitForSeconds = new(Random.Range(0.75f, 1.25f));
        else
            waitForSeconds = new(Random.Range(_minIDLEDuration, _maxIDLEDuration));
        yield return waitForSeconds;
        _currentCoroutine = StartCoroutine(Move());
        Animator.SetTrigger("Move");
        _currentStateIsIDLE = false;
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
        if (!_currentStateIsIDLE)
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
        _currentCoroutine = StartCoroutine(Move());
        Animator.SetTrigger("Move");
        _currentStateIsIDLE = false;
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
        
        _home = homes.OrderBy(home => Vector3.Distance(transform.position, home.DoorTransform.position))
            .FirstOrDefault().DoorTransform.position;
    }

    private void OnHourChangeWhenNotAtHome()
    {
        if (_npc.NpcData.FinishWalkingTime > _npc.NpcData.StartWalkingTime)
        {
            if (((GameTime.Hours > _npc.NpcData.FinishWalkingTime && GameTime.Hours > _npc.NpcData.StartWalkingTime) ||
                (GameTime.Hours < _npc.NpcData.FinishWalkingTime && GameTime.Hours < _npc.NpcData.StartWalkingTime)) == false)
            {
                return;
            }
        }
        else
        {
            if ((GameTime.Hours < _npc.NpcData.StartWalkingTime && GameTime.Hours > _npc.NpcData.FinishWalkingTime) == false)
            {
                return;
            }
        }
        
        GameTime.MinuteChanged += OnMinuteChange;
        GameTime.HourChanged -= OnHourChangeWhenNotAtHome;
        _clickFunctional.enabled = false;
        if (!MoveByNodeIsActive)
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(MoveDirectlyAtHome(_home.x));
            Animator.SetTrigger("Move");
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
            Animator.SetTrigger("Move");
        }
        else
        {
            GameTime.MinuteChanged -= OnMinuteChange;
            GameTime.HourChanged += OnHourChangeWhenAtHome;
            if (_currentCoroutine!= null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
            EnableNPC(false);
            Animator.SetTrigger("IDLE");
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
        if (_npc.NpcData.FinishWalkingTime > _npc.NpcData.StartWalkingTime)
        {
            if ((GameTime.Hours > _npc.NpcData.FinishWalkingTime && GameTime.Hours > _npc.NpcData.StartWalkingTime) ||
                (GameTime.Hours < _npc.NpcData.FinishWalkingTime && GameTime.Hours < _npc.NpcData.StartWalkingTime))
            {
                return;
            }
        }
        else
        {
            if (GameTime.Hours < _npc.NpcData.StartWalkingTime && GameTime.Hours > _npc.NpcData.FinishWalkingTime)
            {
                return;
            }
        }
        
        GameTime.HourChanged -= OnHourChangeWhenAtHome;
        GameTime.HourChanged += OnHourChangeWhenNotAtHome;
        EnableNPC(true);
        if (_currentCoroutine!= null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        StartIDLE(true);
        
    }

    private void EnableNPC(bool enable)
    {
        _spriteRenderer.enabled = enable;
        _collider.enabled = enable;
        _rigidbody.simulated = enable;
        _clickFunctional.enabled = enable;
    }
    
    private IEnumerator MoveDirectlyAtHome(float targetPosX)
    {
        _isGoingToHome = true;
        MoveDistanceAndDirection = targetPosX - transform.position.x;
        StartPosition = transform.position;
        Vector3 targetPosition = new(targetPosX, StartPosition.y);
        WaitForFixedUpdate waitForFixedUpdate = new();
        
        RevertViewDirection(targetPosition.x - StartPosition.x > 0);
        
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
