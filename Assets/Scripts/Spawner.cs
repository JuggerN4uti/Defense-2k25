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
        for (int i = 0; i < 3; i++)
        {
            charge[i] += difficulty[i];
        }
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
        if (charge[0] >= 375)
        {
            charge[0] -= 300;
            difficulty[0] -= 2;
            charge[1] += 6;
            difficulty[1]++;
            charge[2] += 2;
        }
        if (charge[0] >= 75)
        {
            charge[0] -= 75;
            charge[1] += difficulty[1];
            if (charge[1] >= 75)
            {
                charge[1] -= 75;
                difficulty[0]++;
                charge[2] += difficulty[2];
                if (charge[2] >= 75)
                {
                    charge[2] -= 75;
                    difficulty[1]++;
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
