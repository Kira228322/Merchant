using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable<T>
{
    T SaveData();
    void LoadData(T data);

}
