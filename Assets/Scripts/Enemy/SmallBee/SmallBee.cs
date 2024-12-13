using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBee : MonoBehaviour
{
    public float speed = 2.0f;

    Vector3 newPosition;

    void Start()
    {
        PositionChange();
    }

    void PositionChange()
    {
        newPosition = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, newPosition) < 1)
            PositionChange();

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
    }
}
