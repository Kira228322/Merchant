using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class EventInTravel : MonoBehaviour
{
    public string EventName;
    [TextArea(2,6)]public string Description;
    [HideInInspector] public List<string> ButtonsLabel; // задается количество кнопок + их описание 
    public abstract void SetButtons();
    
    public abstract void OnButtonClick(int n);

    protected virtual void SetInteractable() // метод для выключения кнопок действий, которые нельзя совершить
    {
        // этот метод пустой, если нужно ввести ограничения на конкретную кнопку в методе класса это описывается 
    }
}
