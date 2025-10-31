using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyData enemyData;

    protected float currentHP;
    protected float currentDamage;
    protected float currentSpeed;

    protected Transform player;
    protected Rigidbody rb;

    public event Action OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found");
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
        rb.position = new Vector3(rb.position.x, player.position.y, rb.position.z);
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

    void OnCollisionEnter(Collision collision)
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
