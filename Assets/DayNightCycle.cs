using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Camera CameraObject;
    public Spawner SpawnerScript;

    public GameObject TextObject;
    public Transform IconTransform;
    public bool day;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && day)
            StartNight();
    }

    void StartNight()
    {
        day = false;
        CameraObject.backgroundColor = new Color(0.243f, 0.243f, 0.251f);
        TextObject.SetActive(false);
        IconTransform.rotation = Quaternion.Euler(0f, 0f, 180f);
        SpawnerScript.Activate();

        Invoke("StartDay", 50f);
    }

    void StartDay()
    {
        day = true;
        CameraObject.backgroundColor = new Color(0.635f, 0.831f, 0.894f);
        TextObject.SetActive(true);
        IconTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

        SpawnerScript.Deactivate();
    }
}
