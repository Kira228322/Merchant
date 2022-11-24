using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private ContactFilter2D _contactFilter2D;
    private float _speed = 5;
    public float Speed => _speed;
    private Rigidbody2D _rigidbody = new Rigidbody2D();
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public IEnumerator Move(Vector3 startPos,Vector3 targetPos)
    {
        float minDistToLet = 0.2f + _collider.bounds.size.x/2;
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        RaycastHit2D[] raycastHit2D = new RaycastHit2D[8];
        
        Vector2 castDirection = targetPos - startPos + new Vector3(0,0.05f); //+0.05y,чтобы не коллизился с полом
        int count;
        if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, 
                Math.Abs(targetPos.x - startPos.x) + minDistToLet) == 0)
        { // Если препятствий нет
            count = Convert.ToInt32(Math.Abs(targetPos.x - startPos.x) / (_speed * 0.02f));
            for (float i = 1; i <= count; i++) 
            {
                transform.position = new Vector3(math.lerp(startPos.x, targetPos.x, i/count), transform.position.y);
                yield return waitForSeconds;
            }
        }
        else
        {
            Vector2 jumpDirection;
            float distToLet = raycastHit2D[0].distance - minDistToLet;
            if (distToLet > 0)
            {
                count = Convert.ToInt32(distToLet / (_speed * 0.02f));
                
                //
                float targetPosX = raycastHit2D[0].point.x;
                if (targetPosX > startPos.x)
                    targetPosX -= minDistToLet;
                else targetPosX += minDistToLet;
                //
                
                for (float i = 1; i <= count; i++)  
                {
                    transform.position = new Vector3(math.lerp(startPos.x, targetPosX, i/count), transform.position.y);
                    yield return waitForSeconds;
                }
                
                
            }
            
            
            
            
            castDirection = new Vector2(raycastHit2D[0].point.x - transform.position.x, 0).normalized;
            castDirection += new Vector2(0, 2.5f); // кастыль
            if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D,  minDistToLet) == 0)
            {
                if (raycastHit2D[0].point.x > transform.position.x)
                    jumpDirection = new Vector2(0.26f, 0.96f); // 72 градуса
                else jumpDirection = new Vector2(-0.26f, 0.96f);
                    
                _rigidbody.AddForce(jumpDirection * 180);
            }
            
            
        }
    }
}
