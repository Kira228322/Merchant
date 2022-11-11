using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeflow : MonoBehaviour
{
    public float TimeScale; // TimeScale должен влиять на скорость времени суток и на скорость порчи продуктов
    private void Update()
    {
        GameTime.Minutes += Time.deltaTime * TimeScale;
    }
}
