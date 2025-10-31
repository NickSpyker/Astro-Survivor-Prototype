using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    [SerializeField] private GameObject scorePopupPrefab;
    [SerializeField] private Transform scorePopupParent;

    [Header("Wave Info")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    [Header("Combo")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject comboContainer;
    [SerializeField] private Image comboBackground;

    [Header("Kill Streak Messages")]
    [SerializeField] private TextMeshProUGUI killStreakText;
    [SerializeField] private GameObject killStreakContainer;

    private int currentScore = 0;
    private int killCount = 0;
    private int comboCount = 0;
    private float comboTimer = 0f;
    private float comboTimeLimit = 3f;
    private Coroutine comboAnimationCoroutine;

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

        if (killStreakContainer != null)
        {
            killStreakContainer.SetActive(false);
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
        int multipliedPoints = points * (1 + comboCount / 10);
        currentScore += multipliedPoints;
        UpdateScoreDisplay();

        // Spawn score popup
        if (scorePopupPrefab != null && scorePopupParent != null)
        {
            GameObject popup = Instantiate(scorePopupPrefab, scorePopupParent);
            ScorePopup scorePopup = popup.GetComponent<ScorePopup>();
            if (scorePopup != null)
            {
                scorePopup.SetScore(multipliedPoints, comboCount > 1);
            }
        }

        // Animate score text
        if (scoreText != null)
        {
            StartCoroutine(UIAnimations.Pulse(scoreText.transform, 1.2f, 0.2f));
        }
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
            StartCoroutine(UIAnimations.Pulse(killCountText.transform, 1.3f, 0.2f));
        }

        // Show hit marker
        if (HitMarker.Instance != null)
        {
            HitMarker.Instance.ShowHitMarker(false);
        }

        UpdateComboDisplay();
        CheckKillStreak();
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
                bool wasActive = comboContainer.activeSelf;
                comboContainer.SetActive(true);
                comboText.text = $"{comboCount}x COMBO!";

                // Pop in animation when combo first appears
                if (!wasActive)
                {
                    if (comboAnimationCoroutine != null)
                    {
                        StopCoroutine(comboAnimationCoroutine);
                    }
                    comboAnimationCoroutine = StartCoroutine(ComboPopInAnimation());
                }
                else
                {
                    // Just pulse on combo increase
                    StartCoroutine(UIAnimations.Pulse(comboText.transform, 1.4f, 0.25f));
                    StartCoroutine(UIAnimations.RotatePunch(comboText.transform, 10f, 0.25f));
                }

                // Change color based on combo level
                if (comboCount >= 10)
                {
                    comboText.color = Color.red;
                    if (comboCount >= 20)
                    {
                        StartCoroutine(UIAnimations.RainbowCycle(comboText, 0.5f));
                    }
                }
                else if (comboCount >= 5)
                {
                    comboText.color = new Color(1f, 0.5f, 0f); // Orange
                }
                else
                {
                    comboText.color = Color.yellow;
                }
            }
            else
            {
                StartCoroutine(ComboFadeOut());
            }
        }
    }

    private IEnumerator ComboPopInAnimation()
    {
        // Slide in from top with scale
        yield return StartCoroutine(UIAnimations.PopIn(comboContainer.transform, 0.4f, 1.3f));
        yield return StartCoroutine(UIAnimations.ShakeUI(comboContainer.transform, 5f, 0.15f));
    }

    private IEnumerator ComboFadeOut()
    {
        yield return new WaitForSeconds(0.3f);
        if (comboContainer != null)
        {
            comboContainer.SetActive(false);
        }
    }

    private void CheckKillStreak()
    {
        string streakMessage = "";
        bool showStreak = false;

        if (killCount == 5)
        {
            streakMessage = "KILLING SPREE!";
            showStreak = true;
        }
        else if (killCount == 10)
        {
            streakMessage = "RAMPAGE!";
            showStreak = true;
        }
        else if (killCount == 15)
        {
            streakMessage = "DOMINATING!";
            showStreak = true;
        }
        else if (killCount == 20)
        {
            streakMessage = "UNSTOPPABLE!";
            showStreak = true;
        }
        else if (killCount == 30)
        {
            streakMessage = "GODLIKE!";
            showStreak = true;
        }
        else if (killCount % 50 == 0 && killCount > 0)
        {
            streakMessage = "LEGENDARY!";
            showStreak = true;
        }

        if (showStreak && killStreakText != null && killStreakContainer != null)
        {
            StartCoroutine(ShowKillStreakMessage(streakMessage));
        }
    }

    private IEnumerator ShowKillStreakMessage(string message)
    {
        killStreakContainer.SetActive(true);
        killStreakText.text = message;

        // Epic entrance animation
        yield return StartCoroutine(UIAnimations.PopIn(killStreakContainer.transform, 0.5f, 1.5f));
        yield return StartCoroutine(UIAnimations.ShakeUI(killStreakContainer.transform, 10f, 0.2f));

        // Rainbow effect for epic feel
        StartCoroutine(UIAnimations.RainbowCycle(killStreakText, 1f));

        // Hold for 2 seconds
        yield return new WaitForSecondsRealtime(2f);

        // Fade out
        float duration = 0.5f;
        float elapsed = 0f;
        Color originalColor = killStreakText.color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Color c = originalColor;
            c.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            killStreakText.color = c;
            yield return null;
        }

        killStreakContainer.SetActive(false);
        killStreakText.color = originalColor;
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
