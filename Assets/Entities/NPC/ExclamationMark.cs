using System;
using System.Collections;
using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
    private Vector3 startPos;
    public void Init(BoxCollider2D parentCollider)
    {
        if (parentCollider.bounds.size.y > 1.5f)
            gameObject.transform.localScale *= 1.05f;
        startPos = new Vector3(0, parentCollider.bounds.size.y * 1.4f + 0.5f);
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        WaitForSeconds waitForSeconds = new(0.02f);
        while (true)
        {
            transform.localPosition = startPos + new Vector3(0, (float)Math.Sin(1.3f * Time.time) / 3.7f);
            yield return waitForSeconds;
        }
    }
}
