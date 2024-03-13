using System;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    [Header("���� ID ������ ���� ������, ���� ��� ���� �������")]
    [SerializeField] private string _id;

    private void Awake()
    {
        if (_id == string.Empty)
        {
            Debug.LogWarning($"� {gameObject.name} �� ����� �� ������������� UniqueID.");
        }
    }

    public string ID => _id;

    public void GenerateNewID()
    {
        _id = Guid.NewGuid().ToString();
    }
}
