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
    private Rigidbody2D _rigidbody = new Rigidbody2D();
    private Collider2D _collider;
    private float _minDistToLet;
    private Coroutine _currentMove;
    private float _lastValue;

    private float _moveCD = 0.2f;
    private float _currentMoveCD;
    private const float MinConstDist = 0.2f; // минимальное расстояние, которое может возникнуть между игроком и стеной 
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
                Debug.Log("Break");
                _currentMove = null;
            }

            if (_collider.IsTouchingLayers()) // двигаться можно, только если ты не в прыжке
            {
                _currentMove = StartCoroutine(Move(startPos, targetPos));
                Debug.Log("Start");
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
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        RaycastHit2D[] raycastHit2D = new RaycastHit2D[8];

        Vector2 castDirection = new Vector3(targetPos.x - startPos.x, 0.05f); //+0.05y,чтобы не коллизился с полом
        int count;
        if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, 
                Math.Abs(targetPos.x - startPos.x) ) == 0)
        { // Если препятствий нет
            
            count = Convert.ToInt32(Math.Abs(targetPos.x - startPos.x) / (_speed * 0.02f));
            for (float i = 1; i <= count; i++) 
            {
                transform.position = new Vector3(math.lerp(startPos.x, targetPos.x, i/count), transform.position.y);
                yield return waitForSeconds;
                    // проверка если игрок спуститься вниз, то y поменяется и нужно снова отслеживать препятствие
                if (i % 5 == 0)
                    if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, 
                            Math.Abs(targetPos.x - transform.position.x) + _minDistToLet) != 0)
                    {
                        if (raycastHit2D[0].distance > 0.01f) // Это надо, чтобы при спуске вниз, когда объект касается с
                        { // землей не ломалось движение 
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

            count = Convert.ToInt32(Math.Abs(distToLet) / (_speed * 0.02f));
            
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
                waitForSeconds = new WaitForSeconds(0.08f); // задержка перед подпрыгиванием 
                yield return waitForSeconds;
                castDirection = new Vector2(raycastHit2D[0].point.x - transform.position.x, 0).normalized;
                castDirection += new Vector2(0, 2f); // max высота ступеньки, на которую может
                // прыгнуть игрок * (1/MinConstDist) ; пусть высота = 0.4, тогда y = 2
                if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, _minDistToLet) == 0) 
                {
                    if (raycastHit2D[0].point.x > transform.position.x)
                        jumpDirection = new Vector2(0.34f, 0.94f); // 70 градусов
                    else jumpDirection = new Vector2(-0.34f, 0.94f);

                    _rigidbody.AddForce(jumpDirection * 180);
                }

                
                yield return waitForSeconds;
                while (!_collider.IsTouchingLayers()) // ждем пока приземлимся 
                {
                    yield return waitForSeconds;
                }
                
                if (Math.Abs(transform.position.x - targetPos.x) > 0.4f && _lastValue != transform.position.x)
                { // (transform.position.x - targetPos.x) > 0.4f - небольшая погрешность, на всякий случай
                    _lastValue = transform.position.x; // надо если игрок задал движение в недостижимую точку
                    StartCoroutine(Move(transform.position, targetPos));
                }
            }
        }
    }
}
