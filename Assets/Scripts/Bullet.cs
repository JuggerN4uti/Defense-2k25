using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Values")]
    public int pierce;
    public float damage, passDamage;
    public float duration;

    [Header("AoE")]
    public Transform TargetedLocation;
    public GameObject ExplosionAreaObject;
    private Bullet Explosion;
    public float AreaSize, AreaDuration;
    public bool DurationBased;
    //public FallingObject fall;
    float travelX, travelY;
    public bool defuse;

    void Start()
    {
        if (TargetedLocation)
        {
            //duration += 0.01f;
            travelX = (TargetedLocation.position.x - transform.position.x) / duration;
            travelY = (TargetedLocation.position.y - transform.position.y) / duration;
        }

        Invoke("End", duration);
    }

    void Update()
    {
        if (TargetedLocation)
            transform.position = new Vector3(transform.position.x + travelX * Time.deltaTime, transform.position.y + travelY * Time.deltaTime, 0);
    }

    public void Struck()
    {
        AoE();
        pierce--;
        if (pierce <= 0)
            Destroy(gameObject);
        else
            damage *= passDamage;
    }

    void AoE()
    {
        if (ExplosionAreaObject)
        {
            GameObject bullet = Instantiate(ExplosionAreaObject, transform.position, transform.rotation);
            Explosion = bullet.GetComponent(typeof(Bullet)) as Bullet;
            Explosion.damage = damage; //Explosion.DoT = DoT; Explosion.shatter = shatter; Explosion.burn = burn; Explosion.curse = curse;
            Explosion.AreaSize = AreaSize;
            bullet.transform.localScale = new Vector3(AreaSize, AreaSize, 1f);
            if (DurationBased)
                Explosion.duration = AreaDuration;
        }
    }

    void End()
    {
        if (!defuse)
            AoE();
        Destroy(gameObject);
    }
}
