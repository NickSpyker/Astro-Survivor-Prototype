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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        // projectile.GetComponent<EnemyProjectile>().Initialize(2f);
    }
}
