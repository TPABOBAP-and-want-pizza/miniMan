using UnityEngine;

public class SingleRandomNoise : MonoBehaviour
{
    public AudioClip[] noises;
    private AudioSource audioSource;
    [SerializeField] private float noiseLevel = 1.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Метод для проигрывания случайного звука и увеличения уровня шума.
    public void PlayRandomNoise()
    {
        if (noises.Length > 0)
        {
            int index = Random.Range(0, noises.Length);
            audioSource.PlayOneShot(noises[index]);
            // Увеличиваем уровень шума в зависимости от заданной громкости.
            NoiseLevel.Instance.IncreaseNoise(noiseLevel);
        }
    }
}