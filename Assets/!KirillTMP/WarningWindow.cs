using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningWindow : Window
{
    [SerializeField] private TMP_Text _warningLabel;
    [SerializeField] private TMP_Text _warningMassage;
    protected override void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        transform.position = new Vector3(Screen.width - rectTransform.rect.width/2, rectTransform.rect.height/2);
        
        StartCoroutine(AppearenceAnimation(0.5f, 0.02f, rectTransform.rect.height));
    }
    
    public void Init(string label, string massage)
    {
        _warningLabel.text = label;
        _warningMassage.text = massage;
    }

    public void OnButtonClick()
    {
        Destroy(gameObject);
    }
}
