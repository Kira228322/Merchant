using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Node : MonoBehaviour
{
    public Transform AnotherNode; 
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8 ) // Npc
        {
            col.gameObject.GetComponent<NPCMovement>().OnCollisionWithNode(this);
        }
    }
    
    public IEnumerator MoveByNode(NPCMovement movement)
    {
        movement.MoveByNodeIsActive = true;
        movement.Collider.enabled = false;
        movement.StartPosition = movement.transform.position;
        movement.MoveDistanceAndDirection = Random.Range(0.8f, 1.4f) * Math.Sign(AnotherNode.position.x - movement.StartPosition.x); 
        // Random.Range(0.8f, 1.4f) - �������, �������, ��� ����� �������� �� ����� �������� ������� (�� ������
        // ���� �� ������ ���������, �� ��� ����-�� ����������� ������ �����, � ����� �� ������ ���� �� ������)
        // ���� ������ ���� ����� -- ���������, ��� ��������� ������� ��� ������������������
        WaitForFixedUpdate waitForFixedUpdate = new();
        
        float countOfFrames = (AnotherNode.position-movement.StartPosition).magnitude / (movement.Speed * 0.75f * Time.fixedDeltaTime);
        
        for (float i = 0; i < countOfFrames; i++)
        {
            movement.transform.position = Vector3.Lerp(movement.StartPosition, AnotherNode.position, i/countOfFrames);
            yield return  waitForFixedUpdate; // �� ���������� �����������
        }
        
        movement.transform.position = AnotherNode.position;
        
        movement.StartPosition = movement.transform.position;
        Vector3 targetPos = new Vector3(movement.StartPosition.x + movement.MoveDistanceAndDirection, movement.StartPosition.y);

        countOfFrames = Math.Abs(movement.MoveDistanceAndDirection / (movement.Speed * Time.fixedDeltaTime));
        for (float i = 0; i < countOfFrames; i++)
        {
            movement.transform.position = Vector3.Lerp(movement.StartPosition, targetPos, i/countOfFrames);
            yield return  waitForFixedUpdate; // � ����� ���� ������
        }

        movement.transform.position = targetPos;
        movement.Collider.enabled = true;
        movement.MoveByNodeIsActive = false;
        movement.Rigidbody.velocity = Vector2.zero;
        movement.StartIDLE(true);
    }
}
