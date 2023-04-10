using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICraftingItemContainer : MonoBehaviour
{
    public Image ItemIcon;
    public TMPro.TMP_Text ItemAmount;
    public Color BaseColor;
    public Color CompletedColor; //если предметов хватает.
    public void SetCompletedColor(bool value)
    {
        if (value)
            ItemAmount.color = CompletedColor;
        else ItemAmount.color = BaseColor;
    }
}
