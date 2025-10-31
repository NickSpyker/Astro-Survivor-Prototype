using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    public static ParticleEffects Instance { get; private set; }

    [Header("Enemy Death")]
    [SerializeField] private GameObject enemyDeathExplosionPrefab;
    [SerializeField] private float explosionLifetime = 3f;

    [Header("Hit Impact")]
    [SerializeField] private GameObject hitImpactPrefab;
    [SerializeField] private float hitImpactLifetime = 1f;

    [Header("Muzzle Flash")]
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private float muzzleFlashLifetime = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnEnemyDeathExplosion(Vector3 position)
    {
        if (enemyDeathExplosionPrefab != null)
        {
            GameObject explosion = Instantiate(enemyDeathExplosionPrefab, position, Quaternion.identity);
            Destroy(explosion, explosionLifetime);
        }
    }

    public void SpawnHitImpact(Vector3 position, bool isCritical = false)
    {
        if (hitImpactPrefab != null)
        {
            GameObject impact = Instantiate(hitImpactPrefab, position, Quaternion.identity);

            // Make critical hits more dramatic
            if (isCritical)
            {
                ParticleSystem ps = impact.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    var main = ps.main;
                    main.startSize = main.startSize.constant * 1.5f;
                    main.startColor = Color.yellow;
                }
            }

            Destroy(impact, hitImpactLifetime);
        }
    }

    public void SpawnMuzzleFlash(Vector3 position, Quaternion rotation)
    {
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, position, rotation);
            Destroy(muzzleFlash, muzzleFlashLifetime);
        }
    }
}
