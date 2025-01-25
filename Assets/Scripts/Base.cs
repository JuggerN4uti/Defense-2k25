using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public ItemLibrary ILib;
    Bullet BulletScript;

    [Header("Stats")]
    public float maxHitPoints;
    public float hitPoints, regeneration;
    int roll, tempi;
    float temp, size, rotation;

    public Image HealthBarFill;

    [Header("Aim")]
    public Transform Rotation;
    public Transform Barrel, DistancePoint, DistanceRotation, RotatingBarrel;

    [Header("Items")]
    public int[] Item;
    public Rotate OrbsRotateScript;
    int bonusMissile;
    float waveAim, missileFreq;
    bool barraging;

    [Header("Objects")]
    public GameObject FenceObject;
    public GameObject WaveBullet, OrbsOrbitObject, GrenadeObject, MineObject, OrbProjectileObject, ShockwaveObject;
    public GameObject[] OrbsObject;

    [Header("Shockwave")]
    public bool shockwaving;
    public float shockwaveTimer;

    void Start()
    {
        Invoke("Regen", 5f / regeneration);
    }

    void Update()
    {
        if (shockwaving)
        {
            shockwaveTimer -= Time.deltaTime;
            if (shockwaveTimer <= 0f)
                Shockwave();
        }
        if (Input.GetKeyDown(KeyCode.Z))
            GetRandomItem();
        if (Input.GetKeyDown(KeyCode.X))
            CollectItem(Item.Length - 1);
    }

    public void TakeDamage(float value)
    {
        hitPoints -= value;
        HealthBarFill.fillAmount = hitPoints / maxHitPoints;

        if (shockwaving)
            shockwaveTimer -= 0.05f + value * 0.15f;
    }

    void RestoreHealth(float value)
    {
        hitPoints += value;
        if (hitPoints > maxHitPoints)
            hitPoints = maxHitPoints;
        HealthBarFill.fillAmount = hitPoints / maxHitPoints;
    }

    public void GainHP(int value)
    {
        maxHitPoints += value;
        hitPoints += value;
        HealthBarFill.fillAmount = hitPoints / maxHitPoints;
    }

    public void GainRegen(int value)
    {
        regeneration += value;
    }

    void Regen()
    {
        RestoreHealth(1);
        Invoke("Regen", 5f / regeneration);
    }

    public void GetRandomItem()
    {
        roll = Random.Range(0, Item.Length);
        CollectItem(roll);
    }

    public void GetKnownItem()
    {
        do
        {
            roll = Random.Range(0, Item.Length);
        } while (Item[roll] == 0);
        CollectItem(roll);
    }

    public void CollectItem(int itemID)
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
                case 2:
                    Invoke("Item02", 0.2f);
                    break;
                case 3:
                    OrbsOrbitObject.SetActive(true);
                    break;
                case 4:
                    Invoke("Item04", 0.2f);
                    break;
                case 5:
                    Invoke("Item05", 0.2f);
                    break;
                case 6:
                    Invoke("Item06", 0.2f);
                    break;
                case 7:
                    shockwaving = true;
                    break;
                case 8:
                    Invoke("Item08", 0.2f);
                    break;
            }
        }
        switch (itemID)
        {
            case 0:
                SetFence();
                break;
            case 3:
                SetOrbs();
                break;
        }
    }

    public float FenceDamage()
    {
        return (5.5f + 1.8f * Item[0]) * PlayerScript.DamageCalculation(1.25f);
    }

    public void SetFence()
    {
        size = (0.13f + Item[0] * 0.002f) * PlayerScript.SizeCalculation(0.6f);
        FenceObject.transform.localScale = new Vector3(size, size, 1f);
    }

    void Item01()
    {
        tempi = 2 + (3 + 3 * Item[1] + 4 * PlayerScript.projectileCountIncrease) / 9;
        waveAim = Random.Range(0f, 360f);
        for (int i = 0; i < tempi; i++)
        {
            Rotation.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, waveAim + i * (360f / tempi));
            GameObject bullet = Instantiate(WaveBullet, Barrel.position, Barrel.rotation);
            Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
            BulletScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
            BulletScript.damage = Item1Damage();
            BulletScript.pierce = 4 + Item[1] / 4;
            BulletScript.passDamage = 0.9f - (0.7f / (Item[1] + 1));
            bullet_body.AddForce(Barrel.up * 18.2f, ForceMode2D.Impulse);
        }
        Invoke("Item01", 1.2f / PlayerScript.FireRateCalculation());
    }

    public float Item1Damage()
    {
        return (7.9f + 2.1f * Item[1]) * PlayerScript.DamageCalculation();
    }

    void Item02()
    {
        PlayerScript.Item02();

        temp = 1.5f / (1f + 0.06f * Item[2]);
        Invoke("Item02", temp / PlayerScript.FireRateCalculation());
    }

    public float Item2Damage()
    {
        return 6.5f + 1.8f * Item[2];
    }

    public float OrbDamage()
    {
        return (6f + 1.6f * Item[3]) * PlayerScript.DamageCalculation();
    }

    public void SetOrbs()
    {
        size = (1.21f + Item[3] * 0.026f) * PlayerScript.SizeCalculation();
        for (int i = 0; i < 3; i++)
        {
            OrbsObject[i].transform.localScale = new Vector3(size, size, 1f);
        }
        size = 1f * PlayerScript.SizeCalculation(0.1f);
        OrbsOrbitObject.transform.localScale = new Vector3(size, size, 1f);
        rotation = (0.295f + 0.016f * Item[3]) * PlayerScript.DurationCalculation(0.6f);
        OrbsRotateScript.zAngle = -rotation;
    }

    void Item04()
    {
        tempi = 3 + (3 + Item[4] * 4 + PlayerScript.projectileCountIncrease * 6) / 10;
        for (int i = 0; i < tempi; i++)
        {
            Invoke("Item04Launch", i * 0.13f);
        }

        Invoke("Item04", 2.7f / PlayerScript.FireRateCalculation());
    }

    void Item04Launch()
    {
        DistancePoint.position = new Vector2(0f + DistanceRotation.position.x, Random.Range(9.5f * PlayerScript.SizeCalculation(0.5f), 14.2f * PlayerScript.SizeCalculation(1f)) + DistanceRotation.position.y);
        DistanceRotation.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        GameObject bullet = Instantiate(GrenadeObject, transform.position, transform.rotation);
        BulletScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
        BulletScript.TargetedLocation = DistancePoint;
        BulletScript.damage = Item4Damage();
        BulletScript.AreaSize = (0.541f + Item[4] * 0.035f) * PlayerScript.SizeCalculation();
        BulletScript.AreaDuration = (1.59f + Item[4] * 0.09f) * PlayerScript.DurationCalculation();
    }

    public float Item4Damage()
    {
        return (7.3f + 2.16f * Item[4]) * PlayerScript.DamageCalculation();
    }

    void Item05()
    {
        DistancePoint.position = new Vector2(0f + DistanceRotation.position.x, Random.Range(8.8f, 12.4f * PlayerScript.FireRateCalculation(0.3f)) + DistanceRotation.position.y);
        DistanceRotation.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        GameObject bullet = Instantiate(MineObject, transform.position, transform.rotation);
        BulletScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
        BulletScript.TargetedLocation = DistancePoint;
        BulletScript.damage = Item5Damage();
        BulletScript.AreaSize = (0.345f + Item[5] * 0.036f) * PlayerScript.SizeCalculation();
        BulletScript.AreaDuration = (6.77f + Item[5] * 0.224f) * PlayerScript.DurationCalculation();

        temp = 1.55f / (1f + 0.05f * Item[5]);
        Invoke("Item05", temp / PlayerScript.FireRateCalculation(1.12f));
    }

    public float Item5Damage()
    {
        return (10.3f + 2.74f * Item[5]) * PlayerScript.DamageCalculation();
    }

    void Item06()
    {
        GameObject bullet = Instantiate(OrbProjectileObject, RotatingBarrel.position, RotatingBarrel.rotation);
        BulletScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
        Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
        BulletScript.damage = Item6Damage();
        BulletScript.pierce = 1 + (Item[6] * 3 + PlayerScript.projectileCountIncrease * 5) / 11;
        BulletScript.passDamage = 1f - (0.8f / (PlayerScript.projectileCountIncrease + 2));
        bullet_body.AddForce(RotatingBarrel.up * 16.6f, ForceMode2D.Impulse);

        temp = 0.432f / (1f + 0.12f * Item[6] + 0.22f * PlayerScript.projectileCountIncrease);
        Invoke("Item06", temp / PlayerScript.FireRateCalculation(1.04f));
    }

    public float Item6Damage()
    {
        return (7.4f + 2.6f * Item[6]) * PlayerScript.DamageCalculation();
    }

    void Shockwave()
    {
        GameObject bullet = Instantiate(ShockwaveObject, transform.position, transform.rotation);
        BulletScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
        BulletScript.damage = ShockwaveDamage();
        BulletScript.passDamage = 1f - (0.4f / (Item[7] + 1));
        BulletScript.duration = (0.91f + Item[7] * 0.026f) * PlayerScript.DurationCalculation();
        BulletScript.slow = 0.08f + 0.01f * Item[7];

        temp = 5.2f / (1f + 0.04f * Item[7]);
        shockwaveTimer += temp / PlayerScript.FireRateCalculation(0.7f);
    }

    public float ShockwaveDamage()
    {
        return (11.5f + 3.4f * Item[7]) * PlayerScript.DamageCalculation(1.2f);
    }

    void Item08()
    {
        barraging = true;
        missileFreq = 0.13f / (1f + 0.06f * Item[8] + 0.15f * PlayerScript.projectileCountIncrease);
        bonusMissile = 17 + 4 * Item[8] + 11 * PlayerScript.projectileCountIncrease;
        Invoke("Item08Fire", missileFreq);
        Invoke("Item08End", (0.52f + Item[8] * 0.02f) * PlayerScript.DurationCalculation());
    }

    public float Item8Damage()
    {
        return 7.2f + 2.22f * Item[8];
    }

    void Item08Fire()
    {
        PlayerScript.Item08(1 + bonusMissile / 70);
        bonusMissile += 13 + 4 * Item[8] + 6 * PlayerScript.projectileCountIncrease;

        if (barraging)
            Invoke("Item08Fire", missileFreq);
    }

    void Item08End()
    {
        barraging = false;
        temp = 4f / (1f + 0.04f * Item[8]);
        Invoke("Item08", temp / PlayerScript.FireRateCalculation(1.1f));
    }
}
