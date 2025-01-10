using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform SpawnPoint;

    void Start()
    {
        Invoke("Spawn", 3f);
    }

    void Update()
    {
        
    }

    void Spawn()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0f, 360f));
        SpawnPoint.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);

        Instantiate(EnemyPrefab, SpawnPoint.position, SpawnPoint.rotation);

        Invoke("Spawn", 8f);
    }
}
