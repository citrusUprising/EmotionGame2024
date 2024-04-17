using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    
    public float maxDistance = 6.0f;
    Vector3 newPosition;
    public Vector3 setPosition; // if you wanna change the location of this object, change this
    RectTransform rect;
    public float driftSpeed = 0.003f;
    public float driftPause = 0.75f;
    public bool isDrifting = true;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        setPosition = rect.position;
        StartCoroutine(setNewPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDrifting)
            return;
        
        rect.position = Vector3.Lerp(rect.position, newPosition, driftSpeed);
    }

    IEnumerator setNewPosition()
    {
        newPosition = new Vector3(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance), 0);
        newPosition += setPosition;
        yield return new WaitForSeconds(driftPause);
        StartCoroutine(setNewPosition());
    }
}
