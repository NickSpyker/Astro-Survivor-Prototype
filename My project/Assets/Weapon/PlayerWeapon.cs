using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    public string weaponName = "Gatling MK-42";
    public float baseDamage = 10f;
    public float attackSpeed = 0.2f; // Time between shots
    public int projectileCount = 1;
    public float range = 50f;
    public int piercing = 0;
    public float spread = 5f; // Angle spread for multiple projectiles
}

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Configuration")]
    [SerializeField] private WeaponStats weaponStats;
    [SerializeField] private string projectilePoolName = "PlayerBullet";
    [SerializeField] private Transform[] firePoints; // Multiple fire points for multi-barrel weapons

    [Header("Player Stats Reference")]
    [SerializeField] private PlayerStats playerStats; // Reference to player stats for modifiers

    [Header("Audio")]
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioSource audioSource;
    
    private float nextFireTime = 0f;
    private Transform targetEnemy;
    private bool isAutomatic = false;

    private void Update()
    {
        if (isAutomatic)
        {
            AutomaticFire();
        }
    }

    private void AutomaticFire()
    {
        if (Time.time >= nextFireTime)
        {
            targetEnemy = FindNearestEnemy();
            
            if (targetEnemy != null)
            {
                Fire();
                nextFireTime = Time.time + weaponStats.attackSpeed / playerStats.attackSpeed;
            }
        }
    }

    public void Fire()
    {
        // Calculate critical hit
        bool isCritical = Random.value <= playerStats.criticalChances;
        float damageMultiplier = playerStats.damage;
        if (isCritical)
        {
            damageMultiplier *= playerStats.criticalDamage;
        }
        
        // Calculate total projectile count
        int totalProjectiles = weaponStats.projectileCount;
        
        // Fire projectiles
        for (int i = 0; i < totalProjectiles; i++)
        {
            FireSingleProjectile(i, totalProjectiles, damageMultiplier, isCritical);
        }
        
        // Play sound
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    private void FireSingleProjectile(int index, int totalCount, float damageMultiplier, bool isCritical)
    {
        // Select fire point
        Transform firePoint = firePoints.Length > 0 ? firePoints[index % firePoints.Length] : transform;

        // Calculate direction with spread
        Vector3 direction = CalculateProjectileDirection(index, totalCount);

        // Spawn projectile from pool
        GameObject projectileObj = ProjectilePoolManager.Instance.SpawnProjectile(
            projectilePoolName,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        if (projectileObj != null)
        {
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.Initialize(
                ProjectileOwner.Player,
                direction,
                weaponStats.baseDamage,
                damageMultiplier,
                playerStats.range,
                weaponStats.piercing,
                isCritical
            );
        }

        // Juice effects for firing
        if (ParticleEffects.Instance != null)
        {
            ParticleEffects.Instance.SpawnMuzzleFlash(firePoint.position, firePoint.rotation);
        }

        // Very light camera shake on shooting
        if (CameraShake.Instance != null && index == 0) // Only shake once per Fire() call
        {
            CameraShake.Instance.Shake(0.05f, 0.02f, 0.2f);
        }
    }

    private Vector3 CalculateProjectileDirection(int index, int totalCount)
    {
        // Base direction towards target
        Vector3 baseDirection;
        if (targetEnemy != null)
        {
            baseDirection = (targetEnemy.position - transform.position).normalized;
        }
        else
        {
            baseDirection = transform.forward;
        }
        
        // Add spread for multiple projectiles
        if (totalCount > 1)
        {
            float spreadAngle = weaponStats.spread * (index - (totalCount - 1) / 2f);
            Quaternion rotation = Quaternion.AngleAxis(spreadAngle, Vector3.up);
            baseDirection = rotation * baseDirection;
        }
        
        return baseDirection;
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy.transform;
            }
        }
        
        return nearest;
    }
}
