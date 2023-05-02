using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventController<T>
    where T : GlobalEvent_Base
{
    int LastEventDay { get; set; }
    T AddEvent();
}

