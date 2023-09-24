using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DiaryEntry : MonoBehaviour
{
    public TMP_Text HeaderText;
    [HideInInspector] public string DateTimeAcquired;
    [HideInInspector] public string Header;
    [HideInInspector] public string TextInfo;
}
