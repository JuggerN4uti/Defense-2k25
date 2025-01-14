using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTick : MonoBehaviour
{
    public Bullet ThisBulletScript, BulletsScript;
    public GameObject BulletTickObject;
    public Rigidbody2D Dir;
    public Transform Form;
    public float frequency, damagePercent;
    //public int bulletsCount;
    //public float bonusShatter;
    public bool straight, scaleWithAreaSize;

    void Start()
    {
        Invoke("Tick", frequency);
    }

    void Tick()
    {
        if (!straight) //Form.rotation = Quaternion.Euler(Form.rotation.x, Form.rotation.y, Dir.rotation);
            Form.rotation = Quaternion.Euler(Form.rotation.x, Form.rotation.y, Dir.rotation + Random.Range(0f, 360f));
        GameObject bullet = Instantiate(BulletTickObject, Form.position, Form.rotation);
        BulletsScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
        if (scaleWithAreaSize)
            bullet.transform.localScale = new Vector3(ThisBulletScript.AreaSize, ThisBulletScript.AreaSize, 1f);
        SetBullet();
        Invoke("Tick", frequency);
    }

    void SetBullet()
    {
        BulletsScript.damage = ThisBulletScript.damage * damagePercent;
    }
}
