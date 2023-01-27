using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            FindObjectOfType<CanvasWarningGenerator>().CreateWarning
                ("ASld,als dasl;d a,;d", " alsdm; alsdm al;sdm;al smd;lam sd;kam;sdm");
        }
    }
}
