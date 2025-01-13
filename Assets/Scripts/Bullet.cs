using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Values")]
    public int pierce;
    public float damage, passDamage;

    public void Struck()
    {
        pierce--;
        if (pierce <= 0)
            Destroy(gameObject);
    }
}
