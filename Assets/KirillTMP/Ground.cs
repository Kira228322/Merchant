using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ground : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ContactFilter2D _contactFilter2D;
    private Player _player;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private float _speed = 5; // ѕотом это поле будет в скрипте плеер 
    private Coroutine _currentMove;
    
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _rigidbody = _player.GetComponent<Rigidbody2D>();
        _collider = _player.GetComponent<Collider2D>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float y = _player.transform.position.y;
        Vector3 targetPosition = Camera.main.ScreenPointToRay(eventData.position).origin;
        targetPosition = new Vector3(targetPosition.x, y);
        if (_currentMove != null)
        {
            StopCoroutine(_currentMove);
            _currentMove = null;
        }
        _currentMove = StartCoroutine(Move(_player.transform.position,targetPosition));
    }

    private IEnumerator Move(Vector3 startPos,Vector3 targetPos)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        RaycastHit2D[] raycastHit2D = new RaycastHit2D[8];
        Vector2 castDirection = targetPos - startPos + new Vector3(0,0.05f); //+0.05y,чтобы не коллизилс€ с полом
        int count;
        if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, Math.Abs(targetPos.x - startPos.x)) == 0)
        { // ≈сли преп€тствий нет
            count = Convert.ToInt32(Math.Abs(targetPos.x - startPos.x) / (_speed * 0.02f));
            for (float i = 1; i <= count; i++) // именно float 
            {
                _player.transform.position = Vector3.Lerp(startPos, targetPos, i/count);
                yield return waitForSeconds;
            }
        }
        else
        { // ≈сли преп€тствие есть, то двигаемс€ до него
            count = Convert.ToInt32((Math.Abs(raycastHit2D[0].point.x - startPos.x) -  _collider.bounds.size.x/2) / (_speed * 0.02f));
            targetPos = new Vector3(raycastHit2D[0].point.x - _collider.bounds.size.x/2 , targetPos.y);
            for (float i = 1; i < count; i++) // именно < , а не <= , чтобы не врезатьс€ в стенку
            {
                _player.transform.position = Vector3.Lerp(startPos, targetPos, i/count);
                yield return waitForSeconds;
            }
            // ƒалее провер€ем, есть ли дальше ступенька
            castDirection = new Vector2(raycastHit2D[0].point.x - startPos.x, startPos.y);
            castDirection += new Vector2(0, 0.4f); // ¬ысота ступеньки 
            if (_rigidbody.Cast(castDirection, _contactFilter2D, raycastHit2D, Math.Abs(targetPos.x - startPos.x)) == 0)
            {
                
            }
        }
        
        

        _currentMove = null;
    }
}
