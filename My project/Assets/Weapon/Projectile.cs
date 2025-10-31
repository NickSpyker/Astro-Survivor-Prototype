using UnityEngine;

public enum ProjectileOwner
{
    Player,
    Enemy
}

public class Projectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    [SerializeField] public float speed = 50f;
    [SerializeField] public float lifetime = 50f;
    [SerializeField] public float damage = 10f;
    [SerializeField] public int piercing = 0; // 0 = dies after 1 hit, 1 = can hit 2 enemies, etc.
    [SerializeField] public float range = 500f;

    [Header("Visual")]
    [SerializeField] public GameObject impactEffectPrefab;
    [SerializeField] public TrailRenderer trailRenderer;

    private ProjectileOwner owner;
    private Vector3 direction;
    private float traveledDistance = 0f;
    private int piercedCount = 0;
    private float damageMultiplier = 1f;
    private float rangeMultiplier = 1f;
    private bool isCritical = false;

    public void Initialize(
        ProjectileOwner owner,
        Vector3 direction,
        float damage, 
        float damageMultiplier = 1f,
        float rangeMultiplier = 1f, 
        int piercing = 0,
        bool isCritical = false
    )
    {
        this.owner = owner;
        this.direction = direction.normalized;
        this.damage = damage;
        this.damageMultiplier = damageMultiplier;
        this.rangeMultiplier = rangeMultiplier;
        this.piercing = piercing;
        this.isCritical = isCritical;
        
        // Adjust visual for critical hits
        if (isCritical && trailRenderer != null)
        {
            trailRenderer.startColor = Color.yellow;
            trailRenderer.endColor = Color.red;
        }
        
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    private void Update()
    {
        // Move projectile
        float moveDistance = speed * Time.deltaTime;
        transform.position += direction * moveDistance;
        traveledDistance += moveDistance;
        
        // Destroy if out of range
        if (traveledDistance >= range * rangeMultiplier)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collisions based on owner
        if (owner == ProjectileOwner.Player)
        {
            HandlePlayerProjectileCollision(other);
        }
        else if (owner == ProjectileOwner.Enemy)
        {
            HandleEnemyProjectileCollision(other);
        }
    }

    private void HandlePlayerProjectileCollision(Collider other)
    {
        // Check if hit an enemy
        IDamageable damageable = other.GetComponent<IDamageable>();

        Debug.Log("Damageable: " + (damageable != null));
        Debug.Log("Enemy: " + other.CompareTag("Enemy"));

        if (damageable != null && other.CompareTag("Enemy"))
        {
            float finalDamage = damage * damageMultiplier;
            if (isCritical)
            {
                // Critical damage will be calculated in weapon system
                finalDamage *= 2f; // Base critical multiplier
            }

            other.gameObject.GetComponent<Enemy>().TakeDamage(finalDamage, isCritical);
            piercedCount++;

            SpawnImpactEffect(other.transform.position);

            // Destroy after hitting enemy (piercing = 0 means no pierce, destroy after first hit)
            // piercing = 1 means can hit 2 enemies total, etc.
            if (piercing == 0)
            {
                // No piercing - always destroy after first hit
                DestroyProjectile();
            }
            else if (piercedCount > piercing)
            {
                // Has piercing but reached the limit
                DestroyProjectile();
            }
        }
        // Check for obstacles/walls
        else if (other.CompareTag("Obstacle"))
        {
            SpawnImpactEffect(other.ClosestPoint(transform.position));
            DestroyProjectile();
        }
    }

    private void HandleEnemyProjectileCollision(Collider other)
    {
        // Check if hit the player
        if (other.CompareTag("Player"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage * damageMultiplier);
                SpawnImpactEffect(other.transform.position);
                DestroyProjectile();
            }
        }
        // Check for obstacles
        else if (other.CompareTag("Obstacle"))
        {
            SpawnImpactEffect(other.ClosestPoint(transform.position));
            DestroyProjectile();
        }
    }

    private void SpawnImpactEffect(Vector3 position)
    {
        if (impactEffectPrefab != null)
        {
            GameObject impact = Instantiate(impactEffectPrefab, position, Quaternion.identity);
            Destroy(impact, 2f);
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
