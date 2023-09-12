using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Node : MonoBehaviour
{
    public Transform AnotherNode; 
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8 ) // Npc
        {
            col.gameObject.GetComponent<NPCMovement>().OnCollisionWithNode(this);
        }

        if (col.gameObject.layer == 6) // player
        {
            col.gameObject.GetComponent<PlayerMover>().MoveByNode(this);
        }
    }
    
    public IEnumerator MoveNPCByNode(NPCMovement movement)
    {
        movement.MoveByNodeIsActive = true;
        movement.Collider.enabled = false;
        movement.StartPosition = movement.transform.position;
        movement.MoveDistanceAndDirection = Random.Range(1f, 1.4f) * Math.Sign(AnotherNode.position.x - movement.StartPosition.x); 
        // Random.Range(1f, 1.4f) - кастыль, надежда, что после ступенек не будет никакого барьера (По логике
        // если мы ставим ступеньки, то они куда-то обязательно должны вести, и сразу же тупика быть не должно)
        // Если сильно надо будет -- переделаю, так экономнее выходит для производительности
        WaitForFixedUpdate waitForFixedUpdate = new();
        
        float countOfFrames = (AnotherNode.position-movement.StartPosition).magnitude / (movement.Speed * 0.75f * Time.fixedDeltaTime);
        
        for (float i = 0; i < countOfFrames; i++)
        {
            movement.transform.position = Vector3.Lerp(movement.StartPosition, AnotherNode.position, i/countOfFrames);
            yield return  waitForFixedUpdate; // по ступенькам поднимаемся
        }
        
        movement.transform.position = AnotherNode.position;
        
        movement.StartPosition = movement.transform.position;
        Vector3 targetPos = new Vector3(movement.StartPosition.x + movement.MoveDistanceAndDirection, movement.StartPosition.y);

        countOfFrames = Math.Abs(movement.MoveDistanceAndDirection / (movement.Speed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            movement.transform.position = Vector3.Lerp(movement.StartPosition, targetPos, i/countOfFrames);
            yield return  waitForFixedUpdate; // а потом идем дальше
        }

        movement.transform.position = targetPos;
        movement.Collider.enabled = true;
        movement.MoveByNodeIsActive = false;
        movement.Rigidbody.velocity = Vector2.zero;
        movement.StartIDLE(true);
    }
    
    public IEnumerator MovePlayerByNode(PlayerMover movement)
    {
        movement.enabled = false;
        movement._collider.enabled = false;
        Vector3 startPos = movement.transform.position;
        float moveDistanceAndDirection = 1.1f * Math.Sign(AnotherNode.position.x - startPos.x); 
        // 1.1f - размер коллайдера плеера (почему-то через movement._collider.bounds.size.x выдает 0)
        WaitForFixedUpdate waitForFixedUpdate = new();
        
        float countOfFrames = (AnotherNode.position-startPos).magnitude / (movement._currentSpeed * 0.9f * Time.fixedDeltaTime);
        
        for (float i = 0; i < countOfFrames; i++)
        {
            movement.transform.position = Vector3.Lerp(startPos, AnotherNode.position, i/countOfFrames);
            yield return  waitForFixedUpdate; // по ступенькам поднимаемся
        }
        
        movement.transform.position = AnotherNode.position;
        movement._lastNodePos = AnotherNode.position;
        
        startPos = movement.transform.position;
        Vector3 targetPos = new Vector3(startPos.x + moveDistanceAndDirection, startPos.y);
        
        countOfFrames = Math.Abs(moveDistanceAndDirection / (movement._currentSpeed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            movement.transform.position = Vector3.Lerp(startPos, targetPos, i/countOfFrames);
            yield return  waitForFixedUpdate; // а потом идем дальше
        }
        
        movement.transform.position = targetPos;
        movement._collider.enabled = true;
        movement._rigidbody.velocity = Vector2.zero;
        movement.enabled = true;
        movement.MoveAfterNode();
    }
}
