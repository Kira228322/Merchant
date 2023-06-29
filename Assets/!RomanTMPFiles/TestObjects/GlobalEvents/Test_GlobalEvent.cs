using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Test_GlobalEvent : GlobalEvent_Base
{
    public override string GlobalEventName => "Bazugus";
    public override string Description => "Amogus";


    public override void Execute()
    {
        Debug.Log("Executed TestGlobalEvent");
    }

    public override void Terminate()
    {
        Debug.Log("Terminated TestGlobalEvent");
    }
}
