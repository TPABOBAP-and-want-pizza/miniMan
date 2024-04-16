using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseCategory
{
    public string categoryName;
    public AudioClip[] sounds;
}

public class RandomNoise : MonoBehaviour
{
    // Массив для управления категориями и звуками через инспектор Unity.
    [SerializeField] private NoiseCategory[] noiseCategories;

    // Источник аудио для воспроизведения звуков.
    private AudioSource audioSource;

    // Громкость звука, которая будет использоваться для увеличения уровня шума.
    [SerializeField] private float noiseLevel = 1.0f;

    // Словарь для внутреннего использования.
    private Dictionary<string, AudioClip[]> noiseDictionary = new Dictionary<string, AudioClip[]>();

    void Start()
    {
        // Инициализация AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Заполнение словаря из сериализуемого массива
        foreach (var category in noiseCategories)
        {
            noiseDictionary[category.categoryName] = category.sounds;
        }
    }

    public void PlayRandomNoise(string category)
    {
        if (noiseDictionary.ContainsKey(category) && noiseDictionary[category].Length > 0)
        {
            AudioClip[] sounds = noiseDictionary[category];
            int index = Random.Range(0, sounds.Length);
            audioSource.PlayOneShot(sounds[index]);
            NoiseLevel.Instance.IncreaseNoise(noiseLevel);
        }
        else
        {
            Debug.LogWarning("Category not found or no sounds in category: " + category);
        }
    }
}
