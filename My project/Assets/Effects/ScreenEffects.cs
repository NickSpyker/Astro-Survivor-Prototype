using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenEffects : MonoBehaviour
{
    public static ScreenEffects Instance { get; private set; }

    [Header("Damage Flash")]
    [SerializeField] private Image damageFlashImage;
    [SerializeField] private Color damageFlashColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Vignette")]
    [SerializeField] private Image vignetteImage;
    [SerializeField] private Color vignetteColor = new Color(0f, 0f, 0f, 0.5f);

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
        // Make sure images are transparent initially
        if (damageFlashImage != null)
        {
            Color c = damageFlashImage.color;
            c.a = 0f;
            damageFlashImage.color = c;
        }

        if (vignetteImage != null)
        {
            Color c = vignetteImage.color;
            c.a = 0f;
            vignetteImage.color = c;
        }
    }

    public void DamageFlash()
    {
        if (damageFlashImage != null)
        {
            StartCoroutine(DamageFlashCoroutine());
        }
    }

    private IEnumerator DamageFlashCoroutine()
    {
        damageFlashImage.color = damageFlashColor;

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            Color c = damageFlashImage.color;
            c.a = Mathf.Lerp(damageFlashColor.a, 0f, elapsed / flashDuration);
            damageFlashImage.color = c;
            yield return null;
        }

        Color finalColor = damageFlashImage.color;
        finalColor.a = 0f;
        damageFlashImage.color = finalColor;
    }

    public void UpdateVignette(float healthPercent)
    {
        if (vignetteImage != null)
        {
            Color c = vignetteColor;
            // Increase vignette intensity as health gets lower
            c.a = Mathf.Lerp(0.7f, 0f, healthPercent);
            vignetteImage.color = c;
        }
    }
}
