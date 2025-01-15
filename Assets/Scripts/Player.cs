using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Base BaseScript;

    [Header("Movement")]
    public Rigidbody2D Body;
    public float movementSpeed;
    public Transform MoveTowards;
    public Vector2 move;

    [Header("Stats")]
    public bool experienced;
    public int projectileCountIncrease;
    public float damageIncrease, fireRateIncrease, sizeIncrease, durationIncrease;
    int tempi, roll;

    [Header("Aim")]
    public Rigidbody2D Rotation;
    public Transform Hand;
    Vector3 mousePos, mouseVector;

    [Header("Shoot")]
    public GameObject BulletPrefab;
    public GameObject Item2BulletPrefab;
    public Transform Barrel, ItemBarrel;
    public float task;

    [Header("Gun Stats")]
    public float fireRate;
    public float damage, inaccuracy, force, reloadTime;
    public int bullets, magazineSize;
    Bullet BulletScript;

    [Header("Level / Experience")]
    public int level;
    public int experience, expRequired, totalExperience;

    [Header("HUD")]
    public TMPro.TextMeshProUGUI MagazineInfo;

    void Start()
    {
        level = 1;
        expRequired = ExperienceRequiredCalculate();
        if (experienced)
            GainXP(17); //147
        Reload();
    }

    void Update()
    {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (move[0] != 0 || move[1] != 0)
        {
            MoveTowards.position = new Vector3(transform.position.x + move[0] /*+ momentum[0] * 3f*/, transform.position.y + move[1] /*+ momentum[1] * 3f*/, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, MoveTowards.position, movementSpeed * Time.deltaTime);
        }
        Aim();

        if (task > 0f)
            task -= Time.deltaTime * FireRateCalculation(2f);
        else
        {
            if (Input.GetMouseButton(0))
                Shoot();
            else if (Input.GetKeyDown(KeyCode.R) && bullets < magazineSize)
            {
                task += reloadTime;
                Invoke("Reload", reloadTime - 0.05f);
            }
        }
        /*if (move[0] != 0)
        {
            if (momentum[0] > -1f && momentum[0] < 1f)
                momentum[0] += move[0] * Time.deltaTime * 0.5f;
        }
        else
        {
            if (momentum[0] < 0f)
                momentum[0] += Time.deltaTime * 0.2f;
            else if (momentum[0] > 0f)
                momentum[0] -= Time.deltaTime * 0.2f;
        }
        if (move[1] != 0)
        {
            if (momentum[1] > -1f && momentum[1] < 1f)
                momentum[1] += move[1] * Time.deltaTime * 0.5f;
        }
        else
        {
            if (momentum[1] < 0f)
                momentum[1] += Time.deltaTime * 0.2f;
            else if (momentum[1] > 0f)
                momentum[1] -= Time.deltaTime * 0.2f;
        }*/
    }

    void Aim()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        mouseVector = (mousePos - Hand.position).normalized;
        //mouseLeft = Input.GetMouseButton(0);

        float gunAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        Rotation.rotation = gunAngle - 90f;
        /*GunRot.localScale = new Vector3(1f, 1f, 1f);
        Dude.rotation = new Quaternion(0, 0, 0, 0);
        if (Rotation.rotation > 0f || Gun.rotation < -180f)
        {
            GunRot.localScale = new Vector3(-1f, 1f, 1f);
            Dude.rotation = new Quaternion(0, 180, 0, 0);
        }*/
    }

    void Shoot()
    {
        if (bullets > 0)
        {
            task += fireRate;
            bullets--;
            MagazineInfo.text = bullets.ToString() + "/" + magazineSize.ToString();

            tempi = 1 + projectileCountIncrease / 3;
            for (int i = 0; i < tempi; i++)
            {
                Invoke("Fire", i * 0.075f);
            }
        }
        else
        {
            task += reloadTime;
            Invoke("Reload", reloadTime - 0.05f);
        }
    }

    void Fire()
    {
        Barrel.rotation = Quaternion.Euler(Barrel.rotation.x, Barrel.rotation.y, Rotation.rotation + Random.Range(-inaccuracy, inaccuracy));
        GameObject bullet = Instantiate(BulletPrefab, Barrel.position, Barrel.rotation);
        Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
        BulletScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
        BulletScript.damage = damage * DamageCalculation(2f);
        //SetBullet(1f);
        bullet_body.AddForce(Barrel.up * force, ForceMode2D.Impulse);
    }

    void Reload()
    {
        bullets = magazineSize;
        MagazineInfo.text = bullets.ToString() + "/" + magazineSize.ToString();
    }

    void FixedUpdate()
    {

        //Body.velocity = move * movementSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Exp Orb")
        {
            GainXP(1);
            Destroy(other.gameObject);
        }
    }

    void GainXP(int value)
    {
        experience += value;
        totalExperience += value;
        if (experience >= expRequired)
        {
            experience -= expRequired;
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        expRequired = ExperienceRequiredCalculate();
        damage += 0.2f;
        BaseScript.GainHP(5);
        if (level % 5 == 0)
            projectileCountIncrease++;
        else
        {
            roll = Random.Range(0, 4);
            switch (roll)
            {
                case 0:
                    GainDamage(0.024f);
                    break;
                case 1:
                    GainRate(0.03f);
                    break;
                case 2:
                    GainSize(0.036f);
                    break;
                case 3:
                    GainDuration(0.036f);
                    break;
            }
        }
    }

    void GainDamage(float value)
    {
        damageIncrease += value;
    }

    void GainRate(float value)
    {
        fireRateIncrease += value;
    }

    void GainSize(float value)
    {
        sizeIncrease += value;
        if (BaseScript.Item[0] > 0)
            BaseScript.SetFence();
        if (BaseScript.Item[3] > 0)
            BaseScript.SetOrbs();
    }

    void GainDuration(float value)
    {
        durationIncrease += value;
        if (BaseScript.Item[3] > 0)
            BaseScript.SetOrbs();
    }

    // Items
    public void Item02()
    {
        tempi = 4 + (BaseScript.Item[2] * 3 + projectileCountIncrease * 6) / 7;
        for (int i = 0; i < tempi; i++)
        {
            ItemBarrel.rotation = Quaternion.Euler(ItemBarrel.rotation.x, ItemBarrel.rotation.y, Rotation.rotation - (tempi - 1) * 5f + i * 10f);
            GameObject bullet = Instantiate(Item2BulletPrefab, ItemBarrel.position, ItemBarrel.rotation);
            Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
            BulletScript = bullet.GetComponent(typeof(Bullet)) as Bullet;
            BulletScript.damage = BaseScript.Item2Damage() * DamageCalculation();
            bullet_body.AddForce(ItemBarrel.up * 16.8f, ForceMode2D.Impulse);
        }
    }

    // Checks
    int ExperienceRequiredCalculate()
    {
        return 10 + level * 5 + level * level;
    }

    public float DamageCalculation(float efficiency = 1f)
    {
        return 1f + damageIncrease * efficiency;
    }

    public float FireRateCalculation(float efficiency = 1f)
    {
        return 1f + fireRateIncrease * efficiency;
    }

    public float SizeCalculation(float efficiency = 1f)
    {
        return 1f + sizeIncrease * efficiency;
    }

    public float DurationCalculation(float efficiency = 1f)
    {
        return 1f + durationIncrease * efficiency;
    }
}
