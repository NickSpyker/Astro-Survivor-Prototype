using UnityEngine;
using TMPro;
using System.Collections;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float randomOffsetRange = 0.5f;

    private Vector3 moveDirection;
    private Color textColor;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (damageText != null)
        {
            textColor = damageText.color;
        }

        // Random horizontal offset for visual variety
        float randomX = Random.Range(-randomOffsetRange, randomOffsetRange);
        moveDirection = new Vector3(randomX, 1f, 0f).normalized;

        Destroy(gameObject, lifetime);
        StartCoroutine(AnimateNumber());
    }

    void Update()
    {
        // Move upward
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Billboard effect - always face camera
        if (mainCamera != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }
    }

    private IEnumerator AnimateNumber()
    {
        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        // Pop in effect
        while (elapsed < 0.2f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale * 1.2f, elapsed / 0.2f);
            yield return null;
        }

        // Scale back slightly
        elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale * 1.2f, targetScale, elapsed / 0.1f);
            yield return null;
        }

        // Wait before fading
        yield return new WaitForSeconds(lifetime - 0.5f);

        // Fade out
        elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            if (damageText != null)
            {
                Color c = textColor;
                c.a = Mathf.Lerp(1f, 0f, elapsed / 0.5f);
                damageText.color = c;
            }
            yield return null;
        }
    }

    public void SetDamage(float damage, bool isCritical = false)
    {
        if (damageText != null)
        {
            damageText.text = Mathf.RoundToInt(damage).ToString();

            if (isCritical)
            {
                damageText.color = Color.yellow;
                damageText.fontSize = 6f;
                damageText.fontStyle = FontStyles.Bold;
                textColor = Color.yellow;
            }
            else
            {
                damageText.color = Color.white;
                damageText.fontSize = 4f;
                textColor = Color.white;
            }
        }
    }
}
