using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expire : MonoBehaviour
{
    public float duration;

    void Start()
    {
        Invoke("End", duration);
    }

    void End()
    {
        Destroy(gameObject);
    }
}
