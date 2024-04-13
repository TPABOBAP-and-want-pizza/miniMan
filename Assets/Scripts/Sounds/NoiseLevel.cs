using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoiseLevel : MonoBehaviour
{
    public static NoiseLevel Instance { get; private set; }

    [SerializeField] Transform objectTransform;
    private float VolumeIndicator = 0f;
    public float maxNoiseLevel = 100f;
    public Image Bar;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);  // Это делает объект постоянным при загрузке новых сцен.
        }
    }

    void Start()
    {
        Bar.fillAmount = 0f;
        UpdateNoiseLevel();
        StartCoroutine(UpdateNoiseLevelEverySecond());
    }

    public void IncreaseNoise(float amount)
    {
        VolumeIndicator += amount;
        UpdateNoiseLevel();
    }

    public void DecreaseNoise(float amount)
    {
        VolumeIndicator = Mathf.Max(VolumeIndicator - amount, 0);
        UpdateNoiseLevel();
    }

    void UpdateNoiseLevel()
    {
        if (VolumeIndicator >= maxNoiseLevel)
        {
            HandSpawner.SpawnHandDefault(objectTransform.position); 
        }

        // случайное колебание
        float noiseFluctuation = Random.Range(-1.5f, 1f);

        // показатель для Bar.fillAmount с учетом колебания
        float displayVolume = VolumeIndicator + noiseFluctuation;
        displayVolume = Mathf.Clamp(displayVolume, 0, maxNoiseLevel); // Обеспечиваем, чтобы значение не вышло за пределы допустимых

        Bar.fillAmount = displayVolume / maxNoiseLevel;

        //Bar.fillAmount = VolumeIndicator / maxNoiseLevel; старый статичный смособ отображения шума без помех
    }

    IEnumerator UpdateNoiseLevelEverySecond()
    {
        while (true)
        {
            UpdateNoiseLevel(); // Вызываем функцию обновления уровня шума
            yield return new WaitForSeconds(0.1f); // Ожидаем одну секунду
        }
    }
}
