using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeflow : MonoBehaviour
{
    public bool IsTimeFlowing;
    public float TimeScale;
    private void Update()
    {
        if (IsTimeFlowing)
        {
            GameTime.Minutes += Time.deltaTime * TimeScale;
        }
    }
}
