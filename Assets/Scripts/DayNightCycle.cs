using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Camera CameraObject;
    public Spawner SpawnerScript;
    public Base BaseScript;

    public GameObject TextObject;
    public Transform IconTransform;
    public bool day;
    public int cyclesCount;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && day)
            StartNight();
    }

    void StartNight()
    {
        day = false;
        cyclesCount++;
        CameraObject.backgroundColor = new Color(0.243f, 0.243f, 0.251f);
        TextObject.SetActive(false);
        IconTransform.rotation = Quaternion.Euler(0f, 0f, 180f);
        SpawnerScript.frequency = 14.75f / (0.8f + 1.3f * cyclesCount);
        SpawnerScript.difficulty = cyclesCount + (cyclesCount * cyclesCount) / 7;
        SpawnerScript.Activate();

        Invoke("StartDay", 50f);
    }

    void StartDay()
    {
        day = true;
        CameraObject.backgroundColor = new Color(0.635f, 0.831f, 0.894f);
        TextObject.SetActive(true);
        IconTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

        BaseScript.GetRandomItem();
        SpawnerScript.Deactivate();
    }
}
