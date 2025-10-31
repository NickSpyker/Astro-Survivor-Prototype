using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitMarker : MonoBehaviour
{
    public static HitMarker Instance { get; private set; }

    [Header("Hit Marker")]
    [SerializeField] private Image hitMarkerImage;
    [SerializeField] private Color normalHitColor = Color.white;
    [SerializeField] private Color criticalHitColor = Color.yellow;
    [SerializeField] private float hitMarkerDuration = 0.2f;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float criticalScale = 1.5f;

    [Header("Crosshair")]
    [SerializeField] private RectTransform crosshairTransform;
    [SerializeField] private float recoilDistance = 10f;
    [SerializeField] private float recoilDuration = 0.1f;

    private Vector3 crosshairOriginalPosition;
    private Coroutine hitMarkerCoroutine;

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
        if (hitMarkerImage != null)
        {
            Color c = hitMarkerImage.color;
            c.a = 0f;
            hitMarkerImage.color = c;
        }

        if (crosshairTransform != null)
        {
            crosshairOriginalPosition = crosshairTransform.localPosition;
        }
    }

    public void ShowHitMarker(bool isCritical = false)
    {
        if (hitMarkerCoroutine != null)
        {
            StopCoroutine(hitMarkerCoroutine);
        }
        hitMarkerCoroutine = StartCoroutine(HitMarkerAnimation(isCritical));
    }

    private IEnumerator HitMarkerAnimation(bool isCritical)
    {
        if (hitMarkerImage == null) yield break;

        // Set color and scale
        Color targetColor = isCritical ? criticalHitColor : normalHitColor;
        float targetScale = isCritical ? criticalScale : normalScale;

        hitMarkerImage.color = targetColor;
        hitMarkerImage.transform.localScale = Vector3.one * targetScale;

        // Rotate for style
        float randomRotation = Random.Range(-15f, 15f);
        hitMarkerImage.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);

        // Fade out
        float elapsed = 0f;
        while (elapsed < hitMarkerDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / hitMarkerDuration;

            Color c = targetColor;
            c.a = Mathf.Lerp(1f, 0f, t);
            hitMarkerImage.color = c;

            // Scale down slightly
            float scale = Mathf.Lerp(targetScale, targetScale * 0.7f, t);
            hitMarkerImage.transform.localScale = Vector3.one * scale;

            yield return null;
        }

        Color finalColor = hitMarkerImage.color;
        finalColor.a = 0f;
        hitMarkerImage.color = finalColor;
    }

    public void CrosshairRecoil()
    {
        if (crosshairTransform != null)
        {
            StartCoroutine(CrosshairRecoilAnimation());
        }
    }

    private IEnumerator CrosshairRecoilAnimation()
    {
        float elapsed = 0f;

        // Expand crosshair
        while (elapsed < recoilDuration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (recoilDuration / 2f);
            float offset = Mathf.Lerp(0f, recoilDistance, t);
            // You'd expand crosshair parts here if you have separate parts
            yield return null;
        }

        elapsed = 0f;
        // Return to normal
        while (elapsed < recoilDuration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (recoilDuration / 2f);
            float offset = Mathf.Lerp(recoilDistance, 0f, t);
            // Return crosshair parts to normal
            yield return null;
        }
    }
}
