using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Run,

    Attack
};

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentState = EnemyState.Run;
    TextPopUp TextPopUpScript;

    [Header("Movement")]
    public float movementSpeed;
    public float slow;
    public Vector3 center;

    [Header("Health")]
    public float maxHealth, health;
    public Image HealthBarFill;
    Bullet BulletScript;

    [Header("Attack")]
    public float damage;
    public float attackSpeed, task;
    Base BaseScript;

    [Header("Drop")]
    public Rigidbody2D Body;
    public Transform TextOrigin;
    public GameObject ExpOrbPrefab, PopUpObject;
    public int expDropAmount;
    public float expDropChance;

    [Header("Fence")]
    public float fenceDamage;

    void Start()
    {
        health = maxHealth;
        BaseScript = GameObject.FindGameObjectWithTag("Base").GetComponent(typeof(Base)) as Base;
    }

    void Update()
    {
        if (CurrentState == EnemyState.Run)
            transform.position = Vector2.MoveTowards(transform.position, center, MovementSpeed() * Time.deltaTime);
        else
        {
            task -= Time.deltaTime;
            if (task < 0)
                Attack();
        }
    }

    void Attack()
    {
        BaseScript.TakeDamage(damage);
        task += attackSpeed * (1f + slow);
    }

    void TakeDamage(float value)
    {
        health -= value;
        HealthBarFill.fillAmount = health / maxHealth;

        TextOrigin.position = new Vector3(transform.position.x + Random.Range(-0.25f, 0.25f), transform.position.y + Random.Range(-0.25f, 0.25f), 0f);
        TextOrigin.rotation = Quaternion.Euler(TextOrigin.rotation.x, TextOrigin.rotation.y, Body.rotation + Random.Range(-22f, 22f));
        GameObject text = Instantiate(PopUpObject, TextOrigin.position, transform.rotation);
        Rigidbody2D text_body = text.GetComponent<Rigidbody2D>();
        TextPopUpScript = text.GetComponent(typeof(TextPopUp)) as TextPopUp;
        TextPopUpScript.SetText(value, "white");
        text_body.AddForce(TextOrigin.up * Random.Range(1.83f, 2.56f), ForceMode2D.Impulse);

        if (health <= 0f)
            Death();
    }

    void Death()
    {
        for (int i = 0; i < expDropAmount; i++)
        {
            if (expDropChance >= Random.Range(0f, 100f))
            {
                TextOrigin.rotation = Quaternion.Euler(TextOrigin.rotation.x, TextOrigin.rotation.y, Body.rotation + Random.Range(-22f, 22f));
                GameObject exp = Instantiate(ExpOrbPrefab, transform.position, TextOrigin.rotation);
                Rigidbody2D exp_body = exp.GetComponent<Rigidbody2D>();
                exp_body.AddForce(TextOrigin.up * Random.Range(2.09f, 3.02f), ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Base")
            CurrentState = EnemyState.Attack;
        else if (other.transform.tag == "Bullet")
        {
            BulletScript = other.GetComponent(typeof(Bullet)) as Bullet;
            TakeDamage(BulletScript.damage);
            slow += BulletScript.slow;
            BulletScript.Struck();
            //Destroy(other.gameObject);
        }
        else if (other.transform.tag == "Fence")
            EnterFence();
        else if (other.transform.tag == "Orb")
            TakeDamage(BaseScript.OrbDamage());
    }

    void EnterFence()
    {
        fenceDamage = BaseScript.FenceDamage();
        Invoke("FenceTrigger", 0.2f);
    }

    void FenceTrigger()
    {
        TakeDamage(fenceDamage);
        Invoke("FenceTrigger", 1f);
    }

    // Checks
    float MovementSpeed()
    {
        return movementSpeed / (1f + slow);
    }
}
