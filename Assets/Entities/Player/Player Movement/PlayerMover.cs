using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private ContactFilter2D _contactFilter2D;
    private float _speed = 3;
    public float Speed => _speed;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private float _minDistToLet;
    
    private Coroutine _currentMove;
    private float _lastValue;

    private float _moveCD = 0.3f;
    private float _currentMoveCD;
    private const float MinConstDist = 0.2f; // ����������� ����������, ������� ����� ���������� ����� ������� � ������ 
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _minDistToLet = MinConstDist + _collider.bounds.size.x/2; 
        _lastValue = transform.position.x;
    }

    public void StartMove(Vector3 startPos,Vector3 targetPos)
    {
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

            if (_collider.IsTouchingLayers()) // ��������� �����, ������ ���� �� �� � ������
            {
                _currentMove = StartCoroutine(Move(startPos, targetPos));
            }
            
        }
    }

    private IEnumerator ResetMoveCD()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_moveCD);
        yield return waitForSeconds;
        _currentMoveCD = 0;
    }
    
    public IEnumerator Move(Vector3 startPos,Vector3 targetPos)
    {
        bool moveIsDone = false;


        float deltaTime = 0.02f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(deltaTime);
        yield return waitForSeconds;
        RaycastHit2D[] raycastHit2D = new RaycastHit2D[8];

        Vector2 castDirection = new Vector3(targetPos.x - startPos.x, 0.05f); //+0.05y,����� �� ���������� � �����
        int count;
        if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, 
                Math.Abs(targetPos.x - startPos.x) ) == 0)
        { // ���� ����������� ���
            
            count = Convert.ToInt32(Math.Abs(targetPos.x - startPos.x) / (_speed * deltaTime));
            for (float i = 1; i <= count; i++) 
            {
                transform.position = new Vector3(math.lerp(startPos.x, targetPos.x, i/count), transform.position.y);
                yield return waitForSeconds;
                    // �������� ���� ����� ���������� ����, �� y ���������� � ����� ����� ����������� �����������
                if (i % 5 == 0)
                    if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, 
                            Math.Abs(targetPos.x - transform.position.x) + _minDistToLet) != 0)
                    {
                        if (raycastHit2D[0].distance > 0.01f) // ��� ����, ����� ��� ������ ����, ����� ������ �������� �
                        { // ������ �� �������� �������� 
                            StartCoroutine(Move(transform.position, targetPos));
                            break;
                        }
                    }
            }
        }
        else 
        {
            Vector2 jumpDirection;
            float distToLet = raycastHit2D[0].distance - MinConstDist; 

            count = Convert.ToInt32(Math.Abs(distToLet) / (_speed * deltaTime));
            
            float targetPosX = raycastHit2D[0].point.x;
            if (targetPosX > startPos.x)
                targetPosX -= _minDistToLet;
            else targetPosX += _minDistToLet;

            for (float i = 1; i <= count; i++)
            {
                transform.position = new Vector3(math.lerp(startPos.x, targetPosX, i / count), transform.position.y);
                yield return waitForSeconds;
                if (i % 5 == 0)
                    if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D,
                            Math.Abs(targetPosX - transform.position.x) - _minDistToLet) != 0)
                    {
                        if (raycastHit2D[0].distance > 0.01f)
                        {
                            moveIsDone = true;
                            StartCoroutine(Move(transform.position, targetPos));
                            break;
                        }
                    }
            }

            if (!moveIsDone)
            {
                waitForSeconds = new WaitForSeconds(0.07f); // �������� ����� �������������� 
                yield return waitForSeconds;
                castDirection = new Vector2(raycastHit2D[0].point.x - transform.position.x, 0).normalized;
                castDirection += new Vector2(0, 2f); // max ������ ���������, �� ������� �����
                // �������� ����� * (1/MinConstDist) ; ����� ������ = 0.4, ����� y = 2
                if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, _minDistToLet) == 0) 
                {
                    if (raycastHit2D[0].point.x > transform.position.x)
                        jumpDirection = new Vector2(0.34f, 0.94f); // 70 ��������
                    else jumpDirection = new Vector2(-0.34f, 0.94f);

                    _rigidbody.AddForce(jumpDirection * 200);
                }

                
                yield return waitForSeconds;
                while (!_collider.IsTouchingLayers()) // ���� ���� ����������� 
                {
                    yield return waitForSeconds;
                }
                
                if (Math.Abs(transform.position.x - targetPos.x) > 0.4f && _lastValue != transform.position.x)
                { // (transform.position.x - targetPos.x) > 0.4f - ��������� �����������, �� ������ ������
                    _lastValue = transform.position.x; // ���� ���� ����� ����� �������� � ������������ �����
                    yield return waitForSeconds;
                    _currentMove = StartCoroutine(Move(transform.position, targetPos));
                }
            }
        }
    }
}
