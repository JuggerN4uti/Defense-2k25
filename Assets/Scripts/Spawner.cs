using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] EnemyPrefab;
    public Transform SpawnPoint;

    public bool spawning;
    public float frequency;
    public int[] charge, difficulty;

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

        charge[0] += difficulty[0];
        if (charge[0] >= 75)
        {
            charge[0] -= 75;
            charge[1] += difficulty[1];
            if (charge[1] >= 75)
            {
                charge[1] -= 75;
                charge[2] += difficulty[2];
                if (charge[2] >= 75)
                {
                    charge[2] -= 75;
                    Instantiate(EnemyPrefab[3], SpawnPoint.position, SpawnPoint.rotation);
                }
                else Instantiate(EnemyPrefab[2], SpawnPoint.position, SpawnPoint.rotation);
            }
            else Instantiate(EnemyPrefab[1], SpawnPoint.position, SpawnPoint.rotation);
        }
        else Instantiate(EnemyPrefab[0], SpawnPoint.position, SpawnPoint.rotation);

        if (spawning)
            Invoke("Spawn", frequency);
    }
}
