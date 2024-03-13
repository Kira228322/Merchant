using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoSplitSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _currentSliderValueText;
    [SerializeField] private TMP_Text _maxSliderValueText;

    private ItemInfo _parentItemInfo;

    private void Awake()
    {
        _parentItemInfo = GetComponentInParent<ItemInfo>();
    }
    public void Show(InventoryItem currentItem)
    {
        _slider.maxValue = currentItem.CurrentItemsInAStack - 1;
        _maxSliderValueText.text = (currentItem.CurrentItemsInAStack - 1).ToString();

    }
    public void OnSliderValueChanged()
    {
        _currentSliderValueText.text = _slider.value.ToString();
    }
    public void Split()
    {
        _parentItemInfo.Split((int)_slider.value);
    }
}
