using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class WagonPartPanel : MonoBehaviour
{
    [SerializeField] protected Image _image;
    [SerializeField] protected TMP_Text _descriptionText;
    [SerializeField] protected TMP_Text _partNameText;
    [SerializeField] protected TMP_Text _cost;

    public abstract void Init(WagonPart wagonPart);
    // TODO public abstract void OnButtonClick();
}
