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

    private IEnumerator AppearenceAnimation(float duration, float animationFrequency, float appearDistance)
    { // duration must divide by frequency
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        float alpha = canvasGroup.alpha;
        alpha = 0.27f;
        
        Image image = GetComponent<Image>();
        Color color = image.color;
        color.a = 0.75f;
        
        WaitForSeconds frequency = new WaitForSeconds(animationFrequency);
        
        transform.position -= new Vector3(0, appearDistance);
        int count = Convert.ToInt32(duration / animationFrequency);
        Vector3 delta = new Vector3(0,appearDistance/count);
        
        for (int i = 0; i < count; i++)
        {
            transform.position += delta;
            alpha += 0.6f / count; // конечная альфа канвас группы = 0.87 (окна будут немного прозрачные) 
            canvasGroup.alpha = alpha;
            color.a += 0.2f / count; // конечная альфа самого окна, на котором расположены элементы = 0.95
            image.color = color;
            yield return frequency;
        }
    }

    private void CorrectPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        // Image image = GetComponent<Image>();
        
        // transform.position = Input.touches[0].position;
        transform.position = Input.mousePosition;
        switch (rectTransform.position)
        {// Спавним окно не ровно в точке, куда тыкнули, а рядом с ним, чтобы угол окна касался точки нажатия и находился в
            // в пространстве, где больше всего места 
            case Vector3 pos when pos.x > Screen.width/2 && pos.y > Screen.height/2: // 1 четверть
                transform.position += new Vector3(-rectTransform.rect.width, -rectTransform.rect.height)/2;
                break;
            case Vector3 pos when pos.x < Screen.width/2 && pos.y > Screen.height/2: // 2 четверть
                transform.position += new Vector3(rectTransform.rect.width, -rectTransform.rect.height)/2;
                break;
            case Vector3 pos when pos.x < Screen.width/2 && pos.y < Screen.height/2: // 3 четверть
                transform.position += new Vector3(rectTransform.rect.width, rectTransform.rect.height)/2;
                break;
            case Vector3 pos when pos.x > Screen.width/2 && pos.y < Screen.height/2: // 4 четверть
                transform.position += new Vector3(-rectTransform.rect.width, rectTransform.rect.height)/2;
                break;
        }
        
        // Нижний код оказался не нужен, потому что придумал верхний. Этот код пригодится,
        // если будет какое-то широкое или высокое окно, пока закомментим для повышения производительности
        
        // Vector3 delta = new Vector3();
        // if (rectTransform.position.y + rectTransform.rect.height / 2 > Screen.height)
        // { // чтобы окно не заспавнилось вне экрана сверху
        //     delta.y = rectTransform.position.y + rectTransform.rect.height / 2 - Screen.height;
        //     delta.y *= 1.01f;
        //     delta.x = 0;
        //     transform.position -= delta;
        // }
        // else if (rectTransform.position.y - rectTransform.rect.height / 2 < 0)
        // {// чтобы окно не заспавнилось вне экрана снизу
        //     delta.y = rectTransform.position.y - rectTransform.rect.height / 2;
        //     delta.y *= 1.01f;
        //     delta.x = 0;
        //     transform.position -= delta;
        // }
        //
        // if (rectTransform.position.x + rectTransform.rect.width / 2 > Screen.width)
        // {// чтобы окно не заспавнилось вне экрана справа
        //     delta.x = rectTransform.position.x + rectTransform.rect.width / 2  - Screen.width;
        //     delta.x *= 1.01f;
        //     delta.y = 0;
        //     transform.position -= delta;
        // }
        // else if (rectTransform.position.x - rectTransform.rect.width / 2 < 0)
        // {// чтобы окно не заспавнилось вне экрана слева
        //     delta.x = rectTransform.position.x - rectTransform.rect.width / 2;
        //     delta.x *= 1.01f;
        //     delta.y = 0;
        //     transform.position -= delta;
        // }
    }

    private void OnDestroy()
    {
        MapManager.Windows.Remove(this);
    }
}
