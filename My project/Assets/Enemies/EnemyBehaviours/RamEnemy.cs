using UnityEngine;

public class RamEnemy : Enemy
{
    protected override void Move()
    {
        if (player == null) return;
        
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        direction = direction.normalized;
        rb.linearVelocity = direction * currentSpeed;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
