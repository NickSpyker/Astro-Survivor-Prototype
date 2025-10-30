using UnityEngine;

public class GatlingEnemy : Enemy
{
    [Header("Weapon")]
    public GameObject projectilePrefab;
    public float attackCooldown = 0.5f;
    public float keepDistanceRange = 5f;

    private float lastAttackTime;

    protected override void Move()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;
        
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
            Vector2 strafeDirection = new Vector2(-direction.y, direction.x);
            rb.linearVelocity = strafeDirection * currentSpeed * 0.5f;
        }
        
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Shoot();
            lastAttackTime = Time.time;
        }
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        // projectile.GetComponent<EnemyProjectile>().Initialize(2f);
    }
}
