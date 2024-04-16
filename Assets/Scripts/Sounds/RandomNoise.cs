using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseCategory
{
    public string categoryName;
    public AudioClip[] sounds;
    public float noiseLevel = 1.0f; // Уровень шума для данной категории
}


public class RandomNoise : MonoBehaviour
{
    // Массив для управления категориями и звуками через инспектор Unity.
    [SerializeField] private NoiseCategory[] noiseCategories;

    // Источник аудио для воспроизведения звуков.
    private AudioSource audioSource;

    // Словарь для внутреннего использования.
    private Dictionary<string, NoiseCategory> noiseDictionary = new Dictionary<string, NoiseCategory>();

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
            noiseDictionary[category.categoryName] = category;
        }
    }

    public void PlayRandomNoise(string category)
    {
        if (noiseDictionary.ContainsKey(category) && noiseDictionary[category].sounds.Length > 0)
        {
            AudioClip[] sounds = noiseDictionary[category].sounds;
            float categoryNoiseLevel = noiseDictionary[category].noiseLevel;
            int index = Random.Range(0, sounds.Length);
            audioSource.PlayOneShot(sounds[index]);
            NoiseLevel.Instance.IncreaseNoise(categoryNoiseLevel);
        }
        else
        {
            Debug.LogWarning("Category not found or no sounds in category: " + category);
        }
    }
}
