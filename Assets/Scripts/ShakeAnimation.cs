using System.Collections;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour
{
    public float scaleUpAmount = 1.1f;
    public float scaleDuration = 0.1f;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 5f;

    private Vector3 originalScale;
    private Quaternion originalRotation;

    private void OnEnable()
    {
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
        StartCoroutine(AnimateEffect());
    }

    private IEnumerator AnimateEffect()
    {
        Vector3 targetScale = originalScale * scaleUpAmount;
        float t = 0f;
        while (t < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t / scaleDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;

        t = 0f;
        while (t < shakeDuration)
        {
            float zRot = Mathf.Sin(t * 10f) * shakeMagnitude;
            transform.rotation = Quaternion.Euler(0, 0, zRot);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        t = 0f;
        while (t < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t / scaleDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;

        enabled = false;
    }
}
