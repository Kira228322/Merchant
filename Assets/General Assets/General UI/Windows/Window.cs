using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class Window : MonoBehaviour
{
    protected virtual void Start()
    {
        MapManager.Windows.Add(this);

        CorrectPosition();
        StartCoroutine(AppearenceAnimation(0.74f, 0.02f, 50));
    }

    protected IEnumerator AppearenceAnimation(float duration, float animationFrequency, float appearDistance)
    { // duration must divide by frequency
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        float alpha = canvasGroup.alpha;
        alpha = 0.31f;

        Image image = GetComponent<Image>();
        Color color = image.color;
        color.a = 0.74f;

        WaitForSeconds frequency = new(animationFrequency);

        transform.position -= new Vector3(0, appearDistance);
        int count = Convert.ToInt32(duration / animationFrequency);
        Vector3 delta = new(0, appearDistance / count);

        for (int i = 0; i < count; i++)
        {
            transform.position += delta;
            alpha += 0.6f / count; // �������� ����� ������ ������ = 0.91 (���� ����� ������� ����������) 
            canvasGroup.alpha = alpha;
            color.a += 0.2f / count; // �������� ����� ������ ����, �� ������� ����������� �������� = 0.94
            image.color = color;
            yield return frequency;
        }
    }

    private void CorrectPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        // Image image = GetComponent<Image>();

        transform.position = Input.mousePosition;
        switch (rectTransform.position)
        {// ������� ���� �� ����� � �����, ���� �������, � ����� � ���, ����� ���� ���� ������� ����� ������� � ��������� �
            // � ������������, ��� ������ ����� ����� 
            case Vector3 pos when pos.x > Screen.width / 2 && pos.y > Screen.height / 2: // 1 ��������
                transform.position += new Vector3(-rectTransform.rect.width, -rectTransform.rect.height) / 2;
                break;
            case Vector3 pos when pos.x < Screen.width / 2 && pos.y > Screen.height / 2: // 2 ��������
                transform.position += new Vector3(rectTransform.rect.width, -rectTransform.rect.height) / 2;
                break;
            case Vector3 pos when pos.x < Screen.width / 2 && pos.y < Screen.height / 2: // 3 ��������
                transform.position += new Vector3(rectTransform.rect.width, rectTransform.rect.height) / 2;
                break;
            case Vector3 pos when pos.x > Screen.width / 2 && pos.y < Screen.height / 2: // 4 ��������
                transform.position += new Vector3(-rectTransform.rect.width, rectTransform.rect.height) / 2;
                break;
        }

        // ������ ��� �������� �� �����, ������ ��� �������� �������. ���� ��� ����������,
        // ���� ����� �����-�� ������� ��� ������� ����, ���� ����������� ��� ��������� ������������������

        Vector3 delta = new();
        if (rectTransform.position.y + rectTransform.rect.height / 2 > Screen.height)
        { // ����� ���� �� ������������ ��� ������ ������
            delta.y = rectTransform.position.y + rectTransform.rect.height / 2 - Screen.height;
            delta.y *= 1.01f;
            delta.x = 0;
            transform.position -= delta;
        }
        else if (rectTransform.position.y - rectTransform.rect.height / 2 < 0)
        {// ����� ���� �� ������������ ��� ������ �����
            delta.y = rectTransform.position.y - rectTransform.rect.height / 2;
            delta.y *= 1.01f;
            delta.x = 0;
            transform.position -= delta;
        }

        if (rectTransform.position.x + rectTransform.rect.width / 2 > Screen.width)
        {// ����� ���� �� ������������ ��� ������ ������
            delta.x = rectTransform.position.x + rectTransform.rect.width / 2 - Screen.width;
            delta.x *= 1.01f;
            delta.y = 0;
            transform.position -= delta;
        }
        else if (rectTransform.position.x - rectTransform.rect.width / 2 < 0)
        {// ����� ���� �� ������������ ��� ������ �����
            delta.x = rectTransform.position.x - rectTransform.rect.width / 2;
            delta.x *= 1.01f;
            delta.y = 0;
            transform.position -= delta;
        }
    }

    private void OnDestroy()
    {
        MapManager.Windows.Remove(this);
    }
}
