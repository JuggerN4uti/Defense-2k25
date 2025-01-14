using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    public Transform Form;
    public float jump, fall, duration;

    void Start()
    {
        jump *= Random.Range(0.95f, 1.05f);
        fall = jump * 2 / duration;
    }

    void Update()
    {
        Form.position = new Vector3(Form.position.x, Form.position.y + jump * Time.deltaTime, 0);
        jump -= fall * Time.deltaTime;
    }
}
