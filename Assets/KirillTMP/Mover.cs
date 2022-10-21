using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // изначально хотел даное действо сделать в скрипте RandomBGGenerator, но, что делает скрипт Mover не подходит по смыслу
    // для генератора, поэтому такой скрипт... Но потом я хочу как-то с другим функцианалом объедить, пока так
    
    [SerializeField] private float _speed;
    void Update() 
    {
        transform.position += new Vector3(_speed * Time.deltaTime, 0);
    }
}
