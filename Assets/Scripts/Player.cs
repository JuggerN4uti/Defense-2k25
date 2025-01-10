using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D Body;
    public float movementSpeed;
    public Transform MoveTowards;
    public Vector2 move;

    [Header("Aim")]
    public Rigidbody2D Rotation;
    public Transform Hand;
    Vector3 mousePos, mouseVector;

    [Header("Shoot")]
    public GameObject BulletPrefab;
    public Transform Barrel;
    public float task;

    [Header("Gun Stats")]
    public float fireRate;
    public float damage, inaccuracy, force, reloadTime;
    public int bullets, magazineSize;
    Bullet BulletScript;

    [Header("Level / Experience")]
    public int level;
    public int experience, expRequired;

    [Header("HUD")]
    public TMPro.TextMeshProUGUI MagazineInfo;

    void Start()
    {
        level = 1;
        expRequired = ExperienceRequiredCalculate();
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
            task -= Time.deltaTime;
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
            Fire();
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
        BulletScript.damage = damage;
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
        fireRate *= 0.98f;
    }

    // Checks
    int ExperienceRequiredCalculate()
    {
        return 10 + level * 5 + level * level;
    }
}
