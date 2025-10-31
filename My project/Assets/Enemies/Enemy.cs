using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public EnemyData enemyData;

    protected float currentHP;
    protected float currentDamage;
    protected float currentSpeed;

    protected Transform player;
    protected Rigidbody rb;

    public event Action OnDeath;
    public bool IsDead { get; private set; } = false;

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

    // Interface implementation - required by IDamageable
    public void TakeDamage(float damage)
    {
        TakeDamage(damage, false);
    }

    // Overloaded version with critical hit support
    public void TakeDamage(float damage, bool isCritical)
    {
        Debug.Log("Enemy took damage: " + damage);
        currentHP -= damage;
        Debug.Log("Enemy current HP: " + currentHP);

        // Spawn damage number
        if (DamageNumberSpawner.Instance != null)
        {
            Vector3 numberPosition = transform.position + Vector3.up * 2f;
            DamageNumberSpawner.Instance.SpawnDamageNumber(numberPosition, damage, isCritical);
        }

        // Hit impact particle
        if (ParticleEffects.Instance != null)
        {
            ParticleEffects.Instance.SpawnHitImpact(transform.position, isCritical);
        }

        // Light camera shake on hit
        if (CameraShake.Instance != null)
        {
            if (isCritical)
            {
                CameraShake.Instance.ShakeMedium();
            }
            else
            {
                CameraShake.Instance.ShakeLight();
            }
        }

        // Show hit marker
        if (HitMarker.Instance != null)
        {
            HitMarker.Instance.ShowHitMarker(isCritical);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        IsDead = true;

        // Juice effects for death
        if (ParticleEffects.Instance != null)
        {
            ParticleEffects.Instance.SpawnEnemyDeathExplosion(transform.position);
        }

        if (JuiceManager.Instance != null)
        {
            JuiceManager.Instance.EnemyKillJuice();
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.ShakeMedium();
        }

        // Update UI
        if (GameUI.Instance != null)
        {
            GameUI.Instance.AddKill();
        }

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
