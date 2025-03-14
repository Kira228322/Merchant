using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private TMP_Text _ItemName;
    [SerializeField] private Image _craftingStationIcon;
    [SerializeField] private TMP_Text _requiredCraftingStationText;
    [SerializeField] private UICraftingRecipe _recipeUIPrefab;
    [SerializeField] private VerticalLayoutGroup _recipesLayoutGroup;
    [SerializeField] private GameObject _recipeInformationWindow;
    [SerializeField] private Image _resultingItemIcon;
    [SerializeField] private TMP_Text _countOfResultingItem;
    [SerializeField] private TMP_Text _resultingItemDescription;
    [SerializeField] private TMP_Text _requiredCraftingLevelText;
    [SerializeField] private TMP_Text _currentCraftingLevelText;
    [SerializeField] private Button _craftButton;
    [SerializeField] private Image[] _plusSigns = new Image[2];
    [SerializeField] private UICraftingItemContainer[] _requiredItemContainers = new UICraftingItemContainer[3];
    [HideInInspector] public CraftingRecipe SelectedRecipe;
    [SerializeField] private CraftingAnimationPanel _craftingAnimationPanel;
    private CraftingStationType _currentCraftingStation;

    public void OnIconClick()
    {
        ItemInfo itemInfoPanel = Instantiate(_itemInfoPanel, MapManager.Canvas.transform).GetComponent<ItemInfo>();
        itemInfoPanel.Initialize(SelectedRecipe.ResultingItem);
    }

    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        _currentCraftingStation = CraftingStationType.Null;
        Refresh();
    }

    public void SetCraftingStation(Sprite icon, CraftingStationType type)
    {
        GameManager.Instance.CraftingToggle.isOn = true;
        _craftingStationIcon.sprite = icon;
        _currentCraftingStation = type;
        Refresh();
    }

    private string GetRequiredCraftingStationText(CraftingStationType craftingStationType)
    {
        return craftingStationType switch
        {
            CraftingStationType.Campfire => "��������� �������� ����",
            CraftingStationType.CraftingTable => "��������� ���� �������",
            _ => "",
        };
    }
    public void Refresh()
    {
        //������� ����� ���������� �������������
        for (int i = _recipesLayoutGroup.transform.childCount - 1; i >= 0; i--)
            Destroy(_recipesLayoutGroup.transform.GetChild(i).gameObject);
        foreach (var container in _requiredItemContainers)
            container.gameObject.SetActive(false);
        foreach (var plusSign in _plusSigns)
            plusSign.gameObject.SetActive(false);

        _craftingStationIcon.transform.parent.gameObject.SetActive(false);
        //Ÿ parent ��� ������ ������-���������

        _requiredCraftingStationText.gameObject.SetActive(false);

        //������� ���������, ����� ��������� ������

        foreach (CraftingRecipe recipe in Player.Instance.Recipes)
        //���������� ������� �� ������ �����
        {
            GameObject recipeUI = Instantiate(_recipeUIPrefab.gameObject, _recipesLayoutGroup.transform);
            recipeUI.GetComponent<UICraftingRecipe>().Init(this, recipe);
        }

        if (_currentCraftingStation != CraftingStationType.Null)
        {
            _craftingStationIcon.transform.parent.gameObject.SetActive(true);

        }

        if (SelectedRecipe == null)
        {
            ShowCraftingElements(false);
            return;
        }

        ShowCraftingElements(true);

        if (SelectedRecipe.RequiredCraftingStation != CraftingStationType.Null &&
            SelectedRecipe.RequiredCraftingStation != _currentCraftingStation)
        {
            _requiredCraftingStationText.gameObject.SetActive(true);
            _requiredCraftingStationText.text = GetRequiredCraftingStationText(SelectedRecipe.RequiredCraftingStation);
        }

        _countOfResultingItem.text = SelectedRecipe.ResultAmount.ToString();
        _resultingItemIcon.sprite = SelectedRecipe.ResultingItem.Icon;
        _ItemName.text = SelectedRecipe.ResultingItem.Name;
        _resultingItemDescription.text = SelectedRecipe.ResultingItem.Description;
        _requiredCraftingLevelText.text = "��������� ������� ������: " + SelectedRecipe.RequiredCraftingLevel.ToString();
        _currentCraftingLevelText.text = (Player.Instance.Statistics.Crafting.Total < SelectedRecipe.RequiredCraftingLevel ? $"<color=red>" : "") + "������� ������� ������: " + Player.Instance.Statistics.Crafting.Total;
        bool isCraftButtonEnabled = IsCraftButtonEnabled(); //������������� �� ������� ������ � ������� ���������������

        for (int i = 0; i < SelectedRecipe.RequiredItems.Count; i++)
        //���������, ��� i ������� �� ������ 2, ��� � ��� ����� 3 �������� ��� ��������
        {
            if (i > 0) _plusSigns[i - 1].gameObject.SetActive(true);
            int currentCount = Player.Instance.Inventory.GetCount(SelectedRecipe.RequiredItems[i].item);
            if (currentCount < SelectedRecipe.RequiredItems[i].amount)
            {
                isCraftButtonEnabled = false;
            }

            _requiredItemContainers[i].gameObject.SetActive(true);
            _requiredItemContainers[i].ItemIcon.sprite = SelectedRecipe.RequiredItems[i].item.Icon;
            _requiredItemContainers[i].ItemAmount.text = $"{currentCount}/{SelectedRecipe.RequiredItems[i].amount}";

            if (currentCount >= SelectedRecipe.RequiredItems[i].amount)
                _requiredItemContainers[i].SetCompletedColor(true);
            else _requiredItemContainers[i].SetCompletedColor(false);
        }

        if (MapManager.IsActiveSceneTravel)
            _craftButton.interactable = false;
        else
            _craftButton.interactable = isCraftButtonEnabled;
    }

    private bool IsCraftButtonEnabled()
    {
        if (Player.Instance.Statistics.Crafting.Total < SelectedRecipe.RequiredCraftingLevel)
            return false;
        if (SelectedRecipe.RequiredCraftingStation != CraftingStationType.Null &&
            SelectedRecipe.RequiredCraftingStation != _currentCraftingStation)
            return false;
        return true;
    }
    public void OnCraftButtonClick()
    {
        if (Player.Instance.Needs.CurrentHunger == 0 || Player.Instance.Needs.CurrentSleep == 0)
        {
            CanvasWarningGenerator.Instance.CreateWarning("�� ������ ��� �������", "�� �� ������ ��������� ��������, ���� �� ������� ��� ������ �����");
            return;
        }
        if (!InventoryController.Instance.CanInsertItem(SelectedRecipe.ResultingItem, SelectedRecipe.ResultAmount))
        {
            CanvasWarningGenerator.Instance.CreateWarning("������������ �����", "���������� ����� � ���������, ����� ������� �������");
            return;
        }
        foreach (CraftingRecipe.CraftingItem requiredItem in SelectedRecipe.RequiredItems)
        {
            Player.Instance.Inventory.RemoveItemsOfThisItemData(requiredItem.item, requiredItem.amount);
        }

        _craftingAnimationPanel.StartAnimation(SelectedRecipe);

        InventoryController.Instance.TryCreateAndInsertItem
            (SelectedRecipe.ResultingItem, SelectedRecipe.ResultAmount, 0);
        Refresh();
    }
    private void ShowCraftingElements(bool value)
    {
        _recipeInformationWindow.SetActive(value);
    }
}
