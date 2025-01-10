using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Run,

    Attack
};

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentState = EnemyState.Run;

    [Header("Movement")]
    public float movementSpeed;
    public Vector3 center;

    [Header("Health")]
    public float maxHealth, health;
    Bullet BulletScript;

    [Header("Attack")]
    public float damage;
    public float attackSpeed, task;
    Base BaseScript;

    [Header("Drop")]
    public GameObject ExpOrbPrefab;
    public float expDropChance;

    void Start()
    {
        health = maxHealth;
        BaseScript = GameObject.FindGameObjectWithTag("Base").GetComponent(typeof(Base)) as Base;
    }

    void Update()
    {
        if (CurrentState == EnemyState.Run)
            transform.position = Vector2.MoveTowards(transform.position, center, movementSpeed * Time.deltaTime);
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
        task += attackSpeed;
    }

    void TakeDamage(float value)
    {
        health -= value;
        if (health <= 0f)
            Death();
    }

    void Death()
    {
        if (expDropChance >= Random.Range(0f, 100f))
            Instantiate(ExpOrbPrefab, transform.position, transform.rotation);
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
            BulletScript.Struck();
            //Destroy(other.gameObject);
        }
    }
}
