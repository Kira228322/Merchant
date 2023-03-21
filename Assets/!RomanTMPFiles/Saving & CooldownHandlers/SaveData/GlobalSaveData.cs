using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalSaveData 
{
    //GlobalSaveData должна быть сделана, когда будут готовы все остальные сейвдаты.
    //Она должна включать в себя поля всех сохраняемых объектов и быть сохранена через SaveLoadSystem
    //По идее, здесь должны вызываться все SaveData и LoadData у ISaveable объектов
}
