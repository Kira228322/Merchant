using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerMover : MonoBehaviour
{
    [FormerlySerializedAs("_backgroundController")][SerializeField] public BackgroundController BackgroundController;
    [SerializeField] private ContactFilter2D _contactFilter2D;

    [SerializeField] private HoldableButton _holdableButtonRight;
    [SerializeField] private HoldableButton _holdableButtonLeft;


    private float _speed = 3.9f;
    [HideInInspector] public float _currentSpeed;

    public float SpeedModifier = 0;

    [HideInInspector] public Rigidbody2D _rigidbody;
    [HideInInspector] public Collider2D _collider;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Coroutine _currentMove;
    private Coroutine _tickMove;

    private Vector3 _finishTargetPos;
    [HideInInspector] public Vector3 _lastNodePos;

    [FormerlySerializedAs("_wentDistance")]
    [Header("Sound")]
    [HideInInspector] public float WentDistance;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _footSteps;

    private void OnDestroy()
    {
        _holdableButtonRight.IsButtonPressed -= OnRightButtonChangedState;
        _holdableButtonLeft.IsButtonPressed -= OnLeftButtonChangedState;

        MapManager.SceneTransiter.EnteredTravelScene -= HideMoveButtons;
        MapManager.SceneTransiter.EnteredVillageScene -= ShowMoveButtons;
    }

    private void Start()
    {
        _holdableButtonRight.IsButtonPressed += OnRightButtonChangedState;
        _holdableButtonLeft.IsButtonPressed += OnLeftButtonChangedState;

        MapManager.SceneTransiter.EnteredTravelScene += HideMoveButtons;
        MapManager.SceneTransiter.EnteredVillageScene += ShowMoveButtons;
        //Охуенно хитрый костыль также существовал для ShowMoveButtons - это просто переключать их состояние (enabled = !enabled)
        //Потому что после сцены поездки всегда идет сцена ходьбы и наоборот. Но посчитал, что мы пишем
        //не на ассемблере и таких лайфхаков лучше избегать. Поэтому сделал по-нормальному

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        BackgroundController.UpdateBackground(transform.position.x);
        ChangeCurrentSpeed();
    }

    public void DisableMove()
    {
        if (_currentMove != null)
        {
            _animator.SetBool("Move", false);
            StopCoroutine(_currentMove);
            _currentMove = null;
            _rigidbody.velocity = new Vector2(0, 0);
        }

        enabled = false;
    }

    public void EnableMove()
    {
        enabled = true;
    }

    public void StartMove(Vector3 startPos, Vector3 targetPos)
    {
        if (enabled == false)
            return;

        if (_currentMove != null)
        {
            StopCoroutine(_currentMove);
            _currentMove = null;
            _rigidbody.velocity = new Vector2(0, 0);
        }

        _currentMove = StartCoroutine(Move(startPos, targetPos));
    }

    private void ShowMoveButtons()
    {
        _holdableButtonLeft.gameObject.SetActive(true);
        _holdableButtonRight.gameObject.SetActive(true);
    }
    private void HideMoveButtons()
    {
        _holdableButtonLeft.gameObject.SetActive(false);
        _holdableButtonRight.gameObject.SetActive(false);
    }

    private void OnLeftButtonChangedState(bool isPressed)
    {
        if (isPressed && _tickMove == null && enabled)
            _tickMove = StartCoroutine(StartTickMove(false));
        else
        {
            if (_tickMove != null)
            {
                StopCoroutine(_tickMove);
                _tickMove = null;
            }
        }
    }

    private void OnRightButtonChangedState(bool isPressed)
    {
        if (isPressed && _tickMove == null && enabled)
            _tickMove = StartCoroutine(StartTickMove(true));
        else
        {
            if (_tickMove != null)
            {
                StopCoroutine(_tickMove);
                _tickMove = null;
            }
        }
    }

    private IEnumerator StartTickMove(bool rightDirection)
    {
        WaitForSeconds waitForSeconds = new(0.06f);
        float distance;
        if (rightDirection)
            distance = 0.8f;
        else
            distance = -0.8f;
        while (true)
        {
            StartMove(transform.position, new(transform.position.x + distance, transform.position.y));
            yield return waitForSeconds;
        }
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
        if (_lastNodePos.x < transform.position.x && _lastNodePos.x + 0.5f > _finishTargetPos.x)
        {
            _animator.SetBool("Move", false);
            _currentMove = null;
            return;
        }
        if (_lastNodePos.x > transform.position.x && _lastNodePos.x - 0.5f < _finishTargetPos.x)
        {
            _animator.SetBool("Move", false);
            _currentMove = null;
            return;
        }
        // если нужно двигаться дальше
        StartMove(transform.position, _finishTargetPos);
    }

    public IEnumerator Move(Vector3 startPos, Vector3 targetPos)
    {
        RevertPlayer(targetPos.x > startPos.x);
        WaitForEndOfFrame forEndOfFrameUnit = new();
        yield return forEndOfFrameUnit;
        _finishTargetPos = targetPos;
        Vector3 moveDirection;
        if (targetPos.x > startPos.x)
            moveDirection = Vector3.right;
        else
            moveDirection = Vector3.left;

        RaycastHit2D[] raycastHit2D = new RaycastHit2D[8];

        Vector2 castDirection = new(targetPos.x - startPos.x, 0.05f); //+0.05y,чтобы не коллизился с полом

        float distance;

        if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D,
                Math.Abs(targetPos.x - startPos.x)) != 0)
        {
            distance = raycastHit2D[0].distance;
        }
        else
        {
            distance = Math.Abs(targetPos.x - startPos.x);
        }

        if (distance > 0.02f)
        {
            _animator.SetBool("Move", true);

            float travelledDistance = 0f;
            float distanceOfOneStep;
            while (travelledDistance < distance)
            {
                distanceOfOneStep = _currentSpeed * Time.deltaTime;
                travelledDistance += distanceOfOneStep;
                WentDistance += distanceOfOneStep;
                if (WentDistance >= 1.75f) // подобрано эмпирическим путем
                {
                    WentDistance -= 1.75f;
                    PlaySoundOfFootsteps();
                }

                transform.position += distanceOfOneStep * moveDirection;
                BackgroundController.UpdateBackground(transform.position.x);
                yield return forEndOfFrameUnit;
            }

            _animator.SetBool("Move", false);
        }

        _currentMove = null;

    }

    public void PlaySoundOfFootsteps()
    {
        _audioSource.clip = _footSteps[Random.Range(0, _footSteps.Count)];
        _audioSource.PlayOneShotWithRandomPitch();
    }

    private void RevertPlayer(bool rightMove)
    {
        if (rightMove)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;
    }

}
