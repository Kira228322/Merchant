using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPost : EventInTravel
{
    public override void SetButtons()
    {
        // TODO 
        // Если нет контробанды то просто кнопка ехать дальше
        // Если есть, то  3 кнопки такого рода
        // 1.Отдать контробанду
        // 2.предложить {суммарный avgPrice} денег (50% шанс на успех) . Типо предложить немного денег, может примет взятку может нет
        // Если принимает то минус деньги и просто едем дальше. Если не примет, то минус деньги и минус контробанда
        // 3. предложить {суммарный avgPrice * 1.5} денег (80% шанс на успех)
        // Тут на шанс будет влиять дипломатия 
    }

    public override void OnButtonClick(int n)
    {
        throw new System.NotImplementedException();
    }
}
