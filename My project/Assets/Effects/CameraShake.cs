using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isShaking = false;

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
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void Shake(float duration = 0.15f, float magnitude = 0.1f, float rotationMagnitude = 1f)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(duration, magnitude, rotationMagnitude));
        }
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude, float rotationMagnitude)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp01(percentComplete * 1.5f);

            // Random offset for position
            float offsetX = Random.Range(-1f, 1f) * magnitude * damper;
            float offsetY = Random.Range(-1f, 1f) * magnitude * damper;
            float offsetZ = Random.Range(-1f, 1f) * magnitude * damper;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, offsetZ);

            // Random rotation
            float rotX = Random.Range(-1f, 1f) * rotationMagnitude * damper;
            float rotY = Random.Range(-1f, 1f) * rotationMagnitude * damper;
            float rotZ = Random.Range(-1f, 1f) * rotationMagnitude * damper;

            transform.localRotation = originalRotation * Quaternion.Euler(rotX, rotY, rotZ);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset to original
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        isShaking = false;
    }

    public void ShakeLight()
    {
        Shake(0.1f, 0.05f, 0.5f);
    }

    public void ShakeMedium()
    {
        Shake(0.2f, 0.15f, 1.5f);
    }

    public void ShakeHeavy()
    {
        Shake(0.3f, 0.3f, 3f);
    }
}
