using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public Player PlayerScript;
    Bullet BulletScript;

    public float maxHitPoints;
    public float hitPoints;
    int roll;
    float temp;

    public Image HealthBarFill;

    [Header("Aim")]
    public Transform Rotation;
    public Transform Barrel;

    [Header("Items")]
    public int[] Item;

    [Header("Objects")]
    public GameObject FenceObject;
    public GameObject WaveBullet;

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
                case 1:
                    Invoke("Item01", 0.2f);
                    break;
            }
        }
    }

    public float FenceDamage()
    {
        return 5.4f + 1.8f * Item[0];
    }

    void Item01()
    {
        Rotation.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Rotation.rotation.z + Random.Range(0f, 360f));
        GameObject bullet = Instantiate(WaveBullet, Barrel.position, Barrel.rotation);
        Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
        BulletScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
        BulletScript.damage = WaveDamage();
        BulletScript.pierce = 5 + Item[1] / 4;
        BulletScript.passDamage = 1f - (0.4f / Item[1]);
        bullet_body.AddForce(Barrel.up * 18.2f, ForceMode2D.Impulse);
        Invoke("Item01", 0.84f);
    }

    public float WaveDamage()
    {
        return 11.5f + 3f * Item[1];
    }
}
