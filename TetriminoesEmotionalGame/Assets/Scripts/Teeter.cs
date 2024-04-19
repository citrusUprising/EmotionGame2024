using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teeter : MonoBehaviour
{
    
    public float teeterRange = 10.0f;
    public float teeterPause = 0.0f;
    public float teeterDuration = 2.25f;
    float teeterDirection = 1.0f;

    RectTransform rect;
    float maxDelay = 0.25f;
    bool hasGoneOnce = false;


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
        float rotation = 0.0f;
        float oldRotation = rect.eulerAngles.z;
        float newRotation = rect.eulerAngles.z + (teeterRange/2.0f) * teeterDirection;
        float r = 0.0f;
        float time = 0.0f;

        if (!hasGoneOnce)
        {
            yield return new WaitForSeconds(Random.Range(0.0f, maxDelay));
            hasGoneOnce = true;
        }


        while (time < teeterDuration)
        {
            float t = time / teeterDuration;

            t = easeInOutCubic(t);

            r = Mathf.Lerp(oldRotation, newRotation, t);

            rect.eulerAngles = new Vector3(rect.eulerAngles.x, rect.eulerAngles.y, r);

            time += Time.deltaTime;

            yield return null;
        }
        //rect.eulerAngles = new Vector3(rect.eulerAngles.x, rect.eulerAngles.y, newRotation);
        teeterDirection *= -1;

        if (teeterPause > 0.0f)
            yield return new WaitForSeconds(teeterPause);
        
        StartCoroutine(lerp());
    }

    float easeInOutSine(float x) 
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    }

    float easeInOutCubic(float x) 
    {
        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }
}