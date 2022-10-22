using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mover : MonoBehaviour
{
    // ���������� ����� ����� ������� ������� � ������� RandomBGGenerator, ��, ��� ������ ������ Mover �� �������� �� ������
    // ��� ����������, ������� ����� ������... �� ����� � ���� ���-�� � ������ ������������ ��������, ���� ���
    [SerializeField] private RawImage NearBG;
    [SerializeField] private RawImage FarBG;
    [SerializeField] private float _speed;
    public float Speed => _speed;

    private float RawImageChange;
    void Update() 
    {
        transform.position += new Vector3(_speed * Time.deltaTime, 0);
        
        RawImageChange += _speed * Time.deltaTime / 125;
        NearBG.uvRect = new Rect(RawImageChange, 0, NearBG.uvRect.width, NearBG.uvRect.height);
        FarBG.uvRect = new Rect(RawImageChange/4, 0, FarBG.uvRect.width, FarBG.uvRect.height);
    }
}
