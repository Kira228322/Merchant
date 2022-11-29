using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class EventInTravel : MonoBehaviour
{
    public string EventName;
    [TextArea(2,6)]public string Description;
    public List<string> ButtonsLabel; // задается колличество кнопок + их описание 

    public abstract void OnButtonClick();
    
    // Пока сложно, изложу тут свои мысли
    // Видимо для каждого отдетльного события нужен будет скрипт, который наследуется от EventInTravel
    // Внутри этих скриптов уже описывать функционал каждой кнопки с помощью switch от номера нажатой кнопки
    // Возможно придется сделать отдельный скрипт для кнопки во время события в дороге
    // где по факту будет только храниться номер этой самой кнопки
    // Пока буду думать + делать, потом еще раз думать, как можно что-то поменять, удобно ли так будет 
    
}
