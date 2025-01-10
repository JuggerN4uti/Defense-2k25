using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Values")]
    public float damage;

    public void Struck()
    {
        Destroy(gameObject);
    }
}
