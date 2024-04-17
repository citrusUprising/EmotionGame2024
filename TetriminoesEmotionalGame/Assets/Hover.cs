using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float hoverDistance = 20.0f;
    public float hoverDuration = 0.5f;
    float hoverDirection = 1.0f;
    RectTransform rect;

    public bool isHovering = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();

        StartCoroutine(lerp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator lerp()
    {
        float oldPos = rect.localPosition.y;
        float newPos = rect.localPosition.y + (hoverDistance/2.0f) * hoverDirection;
        float y = 0.0f;
        float time = 0.0f;

        while (time < hoverDuration)
        {
            float t = time / hoverDuration;

            t = easeInOutSine(t);

            y = Mathf.Lerp(oldPos, newPos, t);

            rect.localPosition = new Vector3(rect.localPosition.x, y, rect.localPosition.z);

            if (isHovering)
                time += Time.deltaTime;

            yield return null;
        }
        rect.localPosition = new Vector3(rect.localPosition.x, y, rect.localPosition.z);
        hoverDirection *= -1;
        StartCoroutine(lerp());
    }

    float easeInOutSine(float x) 
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    }
}
