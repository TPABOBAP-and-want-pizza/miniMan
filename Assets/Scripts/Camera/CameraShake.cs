using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Transform objectTransform;
    private float shakeDuration = 0.2f; // Продолжительность тряски
    private float shakeMagnitude = 0.2f; // Сила тряски
    //public float shakeDuration = 0.0f; // Продолжительность тряски
    //public float shakeMagnitude = 10f; // Сила тряски
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
            //StartCoroutine(Shake(shakeDuration, shakeMagnitude));
            //DefaultShake();

            //HandSpawner.SpawnHand(transform.localPosition + new Vector3(0, 5, 11), 10);
            //NoiseLevel.IncreaseNoise(10f);
            NoiseLevel.Instance.DecreaseNoise(20f);

        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            NoiseLevel.Instance.IncreaseNoise(20f);

        }
    }

    public void DefaultShake()
    {
        StartCoroutine(Shake(0.2f, 0.2f));
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
    public void TriggerShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }
}
