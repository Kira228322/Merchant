using TMPro;
using UnityEngine;

public class WarningWindow : Window
{
    [SerializeField] private TMP_Text _warningLabel;
    [SerializeField] private TMP_Text _warningMessage;


    private void OnDisable()
    {
        GameManager.Instance.CurrentWarningWindow = null;
    }

    protected override void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 newPosition = new(screenWidth - rectTransform.rect.width, rectTransform.rect.height / 2, 0);
        newPosition.x /= screenWidth;
        newPosition.y /= screenHeight;

        rectTransform.anchorMin = new Vector2(1, 0);
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(1, 0);
        rectTransform.anchoredPosition = newPosition;

        StartCoroutine(AppearenceAnimation(0.5f, 0.02f, rectTransform.rect.height));
    }

    public void Init(string label, string message)
    {
        if (GameManager.Instance.CurrentWarningWindow != null)
            Destroy(GameManager.Instance.CurrentWarningWindow);
        GameManager.Instance.CurrentWarningWindow = gameObject;
        _warningLabel.text = label;
        _warningMessage.text = message;
    }

    public void OnButtonClick()
    {
        Destroy(gameObject);
    }
}
