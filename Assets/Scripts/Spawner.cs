using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform SpawnPoint;

    public bool spawning;

    public void Activate()
    {
        spawning = true;
        Spawn();
    }

    public void Deactivate()
    {
        spawning = false;
    }

    public void Spawn()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0f, 360f));
        SpawnPoint.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);

        Instantiate(EnemyPrefab, SpawnPoint.position, SpawnPoint.rotation);

        if (spawning)
            Invoke("Spawn", 7.5f);
    }
}
