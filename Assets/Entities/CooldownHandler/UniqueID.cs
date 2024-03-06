using System;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    [Header("Поле ID должно быть пустым, если это меню префаба")]
    [SerializeField] private string _id;

    private void Awake()
    {
        if (_id == string.Empty)
        {
            Debug.LogWarning($"У {gameObject.name} на сцене не сгенерировано UniqueID.");
        }
    }

    public string ID => _id;

    public void GenerateNewID()
    {
        _id = Guid.NewGuid().ToString();
    }
}
