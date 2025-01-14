using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] EnemyPrefab;
    public Transform SpawnPoint;

    public bool spawning;
    public float frequency;
    public int charge, difficulty;

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

        charge += difficulty;
        if (charge >= 80)
        {
            Instantiate(EnemyPrefab[1], SpawnPoint.position, SpawnPoint.rotation);
            charge -= 80;
        }
        else Instantiate(EnemyPrefab[0], SpawnPoint.position, SpawnPoint.rotation);

        if (spawning)
            Invoke("Spawn", frequency);
    }
}
