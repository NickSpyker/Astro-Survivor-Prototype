using UnityEngine;

public class RamEnemy : Enemy
{
    protected override void Move()
    {
        if (player == null) return;
        
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * currentSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
