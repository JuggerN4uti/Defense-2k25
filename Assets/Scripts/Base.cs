using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public Player PlayerScript;

    public float maxHitPoints;
    public float hitPoints;
    int roll;

    public Image HealthBarFill;

    [Header("Items")]
    public int[] Item;

    [Header("00")]
    public GameObject FenceObject;
    public float fenceDamage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            GetRandomItem();
    }

    public void TakeDamage(float value)
    {
        hitPoints -= value;
        HealthBarFill.fillAmount = hitPoints / maxHitPoints;
    }

    void GetRandomItem()
    {
        roll = Random.Range(0, Item.Length);
        CollectItem(roll);
    }

    void CollectItem(int itemID)
    {
        Item[itemID]++;
        if (Item[itemID] == 1)
        {
            switch (itemID)
            {
                case 0:
                    FenceObject.SetActive(true);
                    break;
            }
        }
        else
        {
            switch (itemID)
            {
                case 0:
                    fenceDamage += 1.7f;
                    break;
            }
        }
    }
}
