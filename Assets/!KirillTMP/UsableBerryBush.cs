using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableBerryBush : UsableEnvironment
{
    [SerializeField] private Sprite _bushWithoutBerreis;
    protected override void Functional()
    {
        // TODO проверить есть ли место свободное в инвентаре и добавить ягоду какую-нибудь

        GetComponent<SpriteRenderer>().sprite = _bushWithoutBerreis;
    }
}
