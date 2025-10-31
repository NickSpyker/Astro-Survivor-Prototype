using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool invulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 0.5f;

    private float invulnerabilityTimer = 0f;

    // Interface implementation - required by IDamageable
    public bool IsDead { get; private set; } = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0f)
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (invulnerable)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        // Apply juice effects
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.ShakeHeavy();
        }

        if (ScreenEffects.Instance != null)
        {
            ScreenEffects.Instance.DamageFlash();
        }

        if (JuiceManager.Instance != null)
        {
            JuiceManager.Instance.PlayerHitJuice();
        }

        UpdateHealthUI();

        // Brief invulnerability
        invulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (GameUI.Instance != null)
        {
            GameUI.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        IsDead = true;
        Debug.Log("Player died!");
        // TODO: Implement game over logic
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(10f); // Basic collision damage
            }
        }
    }
}
