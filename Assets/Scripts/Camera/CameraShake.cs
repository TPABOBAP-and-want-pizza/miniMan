using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Transform objectTransform;

    public float shakeDuration = 0.0f; // Продолжительность тряски
    public float shakeMagnitude = 10f; // Сила тряски
    private Vector3 shakeOffset = Vector3.zero;
    private Transform playerTransform;

    public void InitShake(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(Shake(shakeDuration, shakeMagnitude));
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude + transform.localPosition.x;
            float y = Random.Range(-1f, 1f) * magnitude + transform.localPosition.y;

            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
