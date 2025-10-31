using UnityEngine;
using System.Collections;

public static class UIAnimations
{
    // Easing functions for smooth animations
    public static float EaseOutElastic(float t)
    {
        float c4 = (2f * Mathf.PI) / 3f;
        return t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
    }

    public static float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    public static float EaseOutBounce(float t)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (t < 1f / d1)
        {
            return n1 * t * t;
        }
        else if (t < 2f / d1)
        {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        }
        else if (t < 2.5f / d1)
        {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        }
        else
        {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }

    public static float EaseInOutCubic(float t)
    {
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }

    // Pop animation - starts from behind (small) and bounces to front
    public static IEnumerator PopIn(Transform target, float duration = 0.5f, float overshoot = 1.3f)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            float easedT = EaseOutBack(t);
            target.localScale = Vector3.Lerp(startScale, targetScale * overshoot, easedT);
            yield return null;
        }

        // Settle back to normal size
        elapsed = 0f;
        float settleTime = 0.1f;
        while (elapsed < settleTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / settleTime;
            target.localScale = Vector3.Lerp(targetScale * overshoot, targetScale, t);
            yield return null;
        }

        target.localScale = targetScale;
    }

    // Pulse animation - grows and shrinks
    public static IEnumerator Pulse(Transform target, float scale = 1.2f, float duration = 0.3f)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * scale;
        float elapsed = 0f;

        // Scale up
        while (elapsed < duration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (duration / 2f);
            target.localScale = Vector3.Lerp(originalScale, targetScale, EaseOutBounce(t));
            yield return null;
        }

        elapsed = 0f;
        // Scale down
        while (elapsed < duration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (duration / 2f);
            target.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        target.localScale = originalScale;
    }

    // Shake animation for UI
    public static IEnumerator ShakeUI(Transform target, float intensity = 10f, float duration = 0.2f)
    {
        Vector3 originalPosition = target.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float damper = 1f - (elapsed / duration);
            float x = Random.Range(-1f, 1f) * intensity * damper;
            float y = Random.Range(-1f, 1f) * intensity * damper;
            target.localPosition = originalPosition + new Vector3(x, y, 0f);
            yield return null;
        }

        target.localPosition = originalPosition;
    }

    // Slide in from side
    public static IEnumerator SlideIn(Transform target, Vector2 fromOffset, float duration = 0.4f)
    {
        Vector3 originalPosition = target.localPosition;
        Vector3 startPosition = originalPosition + new Vector3(fromOffset.x, fromOffset.y, 0f);
        target.localPosition = startPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            target.localPosition = Vector3.Lerp(startPosition, originalPosition, EaseOutBack(t));
            yield return null;
        }

        target.localPosition = originalPosition;
    }

    // Rotate punch effect
    public static IEnumerator RotatePunch(Transform target, float angle = 15f, float duration = 0.3f)
    {
        Quaternion originalRotation = target.localRotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            float currentAngle = angle * Mathf.Sin(t * Mathf.PI * 2f) * (1f - t);
            target.localRotation = originalRotation * Quaternion.Euler(0f, 0f, currentAngle);
            yield return null;
        }

        target.localRotation = originalRotation;
    }

    // Rainbow color cycle
    public static IEnumerator RainbowCycle(UnityEngine.UI.Graphic target, float duration = 1f)
    {
        Color originalColor = target.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float hue = (elapsed / duration) % 1f;
            target.color = Color.HSVToRGB(hue, 1f, 1f);
            yield return null;
        }

        target.color = originalColor;
    }
}
