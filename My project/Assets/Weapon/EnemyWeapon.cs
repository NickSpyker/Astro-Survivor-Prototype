using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Header("Weapon Config")]
    [SerializeField] public float damage = 2f;
    [SerializeField] public float fireRate = 0.5f; // Shots per second
    [SerializeField] public float range = 20f;
    [SerializeField] public string projectilePoolName = "EnemyBullet";
    [SerializeField] public Transform firePoint;
    
    [Header("Targeting")]
    [SerializeField] public float attackRange = 25f;
    [SerializeField] public LayerMask playerLayer;
    
    private Transform player;
    private float nextFireTime = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player != null && CanShoot())
        {
            Fire();
        }
    }

    private bool CanShoot()
    {
        if (Time.time < nextFireTime) return false;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= attackRange;
    }

    private void Fire()
    {
        Vector3 direction = (player.position - firePoint.position).normalized;
        
        GameObject projectileObj = ProjectilePoolManager.Instance.SpawnProjectile(
            projectilePoolName,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );
        
        if (projectileObj != null)
        {
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.Initialize(
                ProjectileOwner.Enemy,
                direction,
                damage,
                1f, // No multiplier for basic enemies
                1f,
                0,  // No piercing
                false
            );
        }
        
        nextFireTime = Time.time + (1f / fireRate);
    }
}
