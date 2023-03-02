using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDisplayIdText : MonoBehaviour
{
    private TMPro.TMP_Text idText;
    private void Start()
    {
        idText = GetComponent<TMPro.TMP_Text>();
        string id = GetComponent<UniqueID>().ID;
        idText.text = id;
    }
}
