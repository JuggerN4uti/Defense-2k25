using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curved : MonoBehaviour
{
    public Transform Point;
    public Rotate RotateScript;
    public float speed;

    void Start()
    {
        RotateScript.zAngle *= Random.Range(0.92f, 1.08f);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Point.position, speed * Time.deltaTime);
    }
}
