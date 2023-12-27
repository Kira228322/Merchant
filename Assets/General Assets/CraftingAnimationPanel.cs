using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingAnimationPanel : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    [SerializeField] private TMP_Text _recipeName;
    [SerializeField] private Image _resultItemImage;
    [SerializeField] private Image _firstItem;
    [SerializeField] private Image _secondItem;
    [SerializeField] private Image _thirdItem;
    [SerializeField] private AudioSource _audioSource;
        
    public void StartAnimation(CraftingRecipe selectedRecipe)
    {
        _parent.SetActive(true);
        _audioSource.PlayOneShot(selectedRecipe._SoundOfCrafting);
        switch (selectedRecipe.RequiredItems.Count)
        {
            case 1:
                _firstItem.gameObject.SetActive(true);
                _firstItem.sprite = selectedRecipe.RequiredItems[0].item.Icon;
                _secondItem.gameObject.SetActive(false);
                _thirdItem.gameObject.SetActive(false);
                break;
            case 2:
                _firstItem.gameObject.SetActive(true);
                _secondItem.gameObject.SetActive(true);
                _firstItem.sprite = selectedRecipe.RequiredItems[0].item.Icon;
                _secondItem.sprite = selectedRecipe.RequiredItems[1].item.Icon;
                _thirdItem.gameObject.SetActive(false);
                break;
            case 3:
                _firstItem.gameObject.SetActive(true);
                _secondItem.gameObject.SetActive(true);
                _thirdItem.gameObject.SetActive(true);
                _firstItem.sprite = selectedRecipe.RequiredItems[0].item.Icon;
                _secondItem.sprite = selectedRecipe.RequiredItems[1].item.Icon;
                _thirdItem.sprite = selectedRecipe.RequiredItems[2].item.Icon;
                break;
        }

        _resultItemImage.sprite = selectedRecipe.ResultingItem.Icon;
        _recipeName.text = "Крафт: " + selectedRecipe.ResultingItem.Name;
    }

    public void AnimationOver()
    {
        _parent.SetActive(false);
    }
}
