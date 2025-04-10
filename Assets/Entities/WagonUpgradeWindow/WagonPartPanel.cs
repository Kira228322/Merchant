using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class WagonPartPanel : MonoBehaviour
{
    [SerializeField] protected Image _image;
    [SerializeField] protected TMP_Text _descriptionText;
    [SerializeField] protected TMP_Text _partNameText;
    [SerializeField] protected TMP_Text _cost;
    [SerializeField] protected Button _installButton;
    [HideInInspector] protected WagonPart _wagonPart;
    [HideInInspector] protected WagonUpgradeWindow _window;

    public abstract void Init(WagonPart wagonPart, WagonUpgradeWindow window);
    public void OnButtonClick()
    {
        if (Player.Instance.Money >= _wagonPart.UpgradePrice)
        {
            _window.AdditiveGoldAnimation.PlayGoldDecrease(_wagonPart.UpgradePrice);
            Player.Instance.WagonStats.UpgradeWagonPart(_wagonPart);
            _window.OnPlayerBoughtAnything(_wagonPart);
        }
        else
        {
            _window.AdditiveGoldAnimation.gameObject.GetComponent<Animator>().SetTrigger("NotEnoughMoney");
        }
    }
}
