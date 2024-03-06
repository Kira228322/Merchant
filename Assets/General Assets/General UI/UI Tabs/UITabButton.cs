using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UITabButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TabGroup _tabGroup;
    [HideInInspector] public Image Background;

    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroup.OnTabSelected(this);
        GameManager.Instance._ButtonAudioSource.PlayOneShot(GameManager.Instance._ButtonAudioSource.clip);
    }

    private void Start()
    {
        Background = GetComponent<Image>();
        _tabGroup.Subscribe(this);
    }
}
