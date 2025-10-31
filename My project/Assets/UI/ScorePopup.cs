using UnityEngine;
using TMPro;
using System.Collections;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float lifetime = 1.5f;
    [SerializeField] private float moveUpSpeed = 100f;

    private RectTransform rectTransform;
    private Color textColor;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (scoreText != null)
        {
            textColor = scoreText.color;
        }

        StartCoroutine(AnimateScorePopup());
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += Vector2.up * moveUpSpeed * Time.unscaledDeltaTime;
        }
    }

    public void SetScore(int score, bool isCombo = false)
    {
        if (scoreText != null)
        {
            scoreText.text = $"+{score}";

            if (isCombo)
            {
                scoreText.color = Color.yellow;
                scoreText.fontSize = 48;
                textColor = Color.yellow;
            }
            else
            {
                scoreText.color = Color.white;
                scoreText.fontSize = 36;
                textColor = Color.white;
            }
        }
    }

    private IEnumerator AnimateScorePopup()
    {
        // Pop in
        transform.localScale = Vector3.zero;
        float elapsed = 0f;
        float popDuration = 0.3f;

        while (elapsed < popDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / popDuration;
            transform.localScale = Vector3.one * UIAnimations.EaseOutBack(t);
            yield return null;
        }

        // Wait
        yield return new WaitForSecondsRealtime(lifetime - popDuration - 0.5f);

        // Fade out
        elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            if (scoreText != null)
            {
                Color c = textColor;
                c.a = Mathf.Lerp(1f, 0f, elapsed / 0.5f);
                scoreText.color = c;
            }
            yield return null;
        }
    }
}
