using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient healthColorGradient;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, 0);
    [SerializeField] private bool showOnlyWhenDamaged = true;
    [SerializeField] private float hideDelay = 2f;

    private Camera mainCamera;
    private Enemy enemy;
    private Canvas canvas;
    private float maxHealth;
    private float hideTimer = 0f;
    private bool isVisible = false;

    void Start()
    {
        mainCamera = Camera.main;
        enemy = GetComponentInParent<Enemy>();

        if (enemy != null && enemy.enemyData != null)
        {
            maxHealth = enemy.enemyData.baseHP;
        }

        canvas = GetComponentInChildren<Canvas>();
        if (showOnlyWhenDamaged && canvas != null)
        {
            canvas.enabled = false;
        }
    }

    void Update()
    {
        // Billboard effect
        if (mainCamera != null && canvas != null)
        {
            transform.position = enemy.transform.position + offset;
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }

        // Auto-hide timer
        if (isVisible && showOnlyWhenDamaged)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
            {
                HideHealthBar();
            }
        }
    }

    public void UpdateHealthBar(float currentHealth)
    {
        if (healthBar != null)
        {
            float healthPercent = currentHealth / maxHealth;
            healthBar.value = healthPercent;

            if (fillImage != null && healthColorGradient != null)
            {
                fillImage.color = healthColorGradient.Evaluate(healthPercent);
            }
        }

        // Show health bar when damaged
        if (showOnlyWhenDamaged && currentHealth < maxHealth)
        {
            ShowHealthBar();
        }
    }

    private void ShowHealthBar()
    {
        if (canvas != null)
        {
            canvas.enabled = true;
            isVisible = true;
            hideTimer = hideDelay;
        }
    }

    private void HideHealthBar()
    {
        if (canvas != null)
        {
            canvas.enabled = false;
            isVisible = false;
        }
    }
}
