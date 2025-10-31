using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Gradient healthColorGradient;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI killCountText;

    [Header("Wave Info")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    [Header("Combo")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject comboContainer;

    private int currentScore = 0;
    private int killCount = 0;
    private int comboCount = 0;
    private float comboTimer = 0f;
    private float comboTimeLimit = 3f;

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

    void Start()
    {
        if (comboContainer != null)
        {
            comboContainer.SetActive(false);
        }
    }

    void Update()
    {
        // Update combo timer
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    public void UpdateHealth(float current, float max)
    {
        if (healthBar != null)
        {
            healthBar.value = current / max;
        }

        if (healthFill != null && healthColorGradient != null)
        {
            healthFill.color = healthColorGradient.Evaluate(current / max);
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
        }

        // Update vignette based on health
        if (ScreenEffects.Instance != null)
        {
            ScreenEffects.Instance.UpdateVignette(current / max);
        }
    }

    public void AddScore(int points)
    {
        currentScore += points * (1 + comboCount / 10);
        UpdateScoreDisplay();
    }

    public void AddKill()
    {
        killCount++;
        comboCount++;
        comboTimer = comboTimeLimit;

        // Update displays
        if (killCountText != null)
        {
            killCountText.text = $"Kills: {killCount}";
        }

        UpdateComboDisplay();
        AddScore(100); // Base score per kill
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }

    private void UpdateComboDisplay()
    {
        if (comboText != null && comboContainer != null)
        {
            if (comboCount > 1)
            {
                comboContainer.SetActive(true);
                comboText.text = $"{comboCount}x COMBO!";

                // Pulse effect for combo
                float scale = 1f + (comboCount * 0.05f);
                comboText.transform.localScale = Vector3.one * Mathf.Min(scale, 2f);
            }
            else
            {
                comboContainer.SetActive(false);
            }
        }
    }

    private void ResetCombo()
    {
        comboCount = 0;
        if (comboContainer != null)
        {
            comboContainer.SetActive(false);
        }
    }

    public void UpdateWaveInfo(int waveNumber, int enemiesRemaining)
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {waveNumber}";
        }

        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies: {enemiesRemaining}";
        }
    }
}
