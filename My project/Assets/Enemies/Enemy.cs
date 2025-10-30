using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyData enemyData;

    protected float currentHP;
    protected float currentDamage;
    protected float currentSpeed;

    protected Transform player;
    protected Rigidbody2D rb;

    public event Action OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found");
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found");
            return;
        }

        currentHP = enemyData.baseHP;
        currentDamage = enemyData.baseDamage;
        currentSpeed = enemyData.baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected virtual void Move() { /* To be overridden */ }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // TODO: Player take damage
        }
    }

    public void ApplyZoneScaling(int zoneNumber)
    {
        float scalingMultiplier = Mathf.Pow(1.2f, zoneNumber - 1);
        currentHP = enemyData.baseHP * scalingMultiplier;
        currentDamage = enemyData.baseDamage * scalingMultiplier;
        currentSpeed = enemyData.baseSpeed * scalingMultiplier;
    }
}
