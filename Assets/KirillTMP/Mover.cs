using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // ���������� ����� ����� ������� ������� � ������� RandomBGGenerator, ��, ��� ������ ������ Mover �� �������� �� ������
    // ��� ����������, ������� ����� ������... �� ����� � ���� ���-�� � ������ ������������ ��������, ���� ���
    
    [SerializeField] private float _speed;
    void Update() 
    {
        transform.position += new Vector3(_speed * Time.deltaTime, 0);
    }
}
