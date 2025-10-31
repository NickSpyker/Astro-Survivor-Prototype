using UnityEngine;

public class GatlingEnemy : Enemy
{
    [Header("Weapon")]
    public float attackCooldown = 0.5f;
    public float keepDistanceRange = 10f;
    public Transform firePoint;
    public string projectilePoolName = "EnemyBullet";
    public float projectileDamage = 5f;

    private float lastAttackTime;

    protected override void Move()
    {
        if (player == null) return;
        
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float distanceToPlayer = toPlayer.magnitude;
        Vector3 direction = distanceToPlayer > 0f ? toPlayer / distanceToPlayer : Vector3.zero;
        
        if (distanceToPlayer < keepDistanceRange)
        {
            rb.linearVelocity = -direction * currentSpeed;
        }
        else if (distanceToPlayer > keepDistanceRange * 1.5f)
        {
            rb.linearVelocity = direction * currentSpeed;
        }
        else
        {
            Vector3 strafeDirection = Vector3.Cross(Vector3.up, direction).normalized;
            rb.linearVelocity = strafeDirection * currentSpeed * 0.5f;
        }
        
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Shoot();
            lastAttackTime = Time.time;
        }
        
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    void Shoot()
    {
        if (player == null) return;

        // Use firePoint if available, otherwise use transform position
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + transform.forward * 1f;
        Vector3 direction = (player.position - spawnPosition).normalized;

        // Check if ProjectilePoolManager exists
        if (ProjectilePoolManager.Instance != null)
        {
            GameObject projectileObj = ProjectilePoolManager.Instance.SpawnProjectile(
                projectilePoolName,
                spawnPosition,
                Quaternion.LookRotation(direction)
            );

            if (projectileObj != null)
            {
                Projectile projectile = projectileObj.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.Initialize(
                        ProjectileOwner.Enemy,
                        direction,
                        projectileDamage,
                        1f,     // No multiplier
                        1f,     // No range multiplier
                        0,      // No piercing
                        false   // Not critical
                    );
                }
            }
        }
        else
        {
            Debug.LogWarning("ProjectilePoolManager not found! Make sure it exists in the scene.");
        }

        // Spawn muzzle flash if available
        if (ParticleEffects.Instance != null)
        {
            ParticleEffects.Instance.SpawnMuzzleFlash(spawnPosition, transform.rotation);
        }
    }
}
