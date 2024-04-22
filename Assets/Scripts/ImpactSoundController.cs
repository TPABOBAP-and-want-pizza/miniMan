using UnityEngine;

public class ImpactSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] impactSounds;
    [SerializeField] private float minImpactVolume = 0.1f;
    [SerializeField] private float maxImpactVolume = 1.0f;
    [SerializeField] private float minForce = 10.0f;
    [SerializeField] private float soundDelay = 0.5f;
    private float lastSoundTime = 0.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float currentTime = Time.time;
        if (currentTime - lastSoundTime >= soundDelay)
        {
            float impactForce = collision.relativeVelocity.magnitude;
            if (impactForce >= minForce)
            {
                float volume = Mathf.Clamp(impactForce/10, minImpactVolume, maxImpactVolume);
                AudioClip randomImpactSound = impactSounds[Random.Range(0, impactSounds.Length)];
                NoiseLevel.Instance.IncreaseNoise(volume*20, gameObject);
                AudioSource.PlayClipAtPoint(randomImpactSound, transform.position, volume);
                lastSoundTime = currentTime;
            }
        }
    }
}
