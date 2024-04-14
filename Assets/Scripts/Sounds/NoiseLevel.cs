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
    public Image[] noisePoints;

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
        StartCoroutine(UpdateNoiseLevelEverySecond());//отображение шума
        StartCoroutine(DecreaseNoiseLevelOverTime());// Уменьшение шума каждую секунду
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
            HandSpawner.SpawnHandDefault(transform.localPosition);
            // HandSpawner.SpawnHand(transform.localPosition + new Vector3(0, 5, 11), 10);
            DecreaseNoise(50f);
        }

        // случайное колебание
        float noiseFluctuation = Random.Range(-6f, 6f);

        // показатель для Bar.fillAmount с учетом колебания
        float displayVolume = VolumeIndicator + noiseFluctuation;
        displayVolume = Mathf.Clamp(displayVolume, 0, maxNoiseLevel); // не вышло за пределы допустимых

        Bar.fillAmount = displayVolume / maxNoiseLevel;

        for (int i = 0; i < noisePoints.Length; i++)
        {
            noisePoints[i].enabled = !DisplayNoisePoint(displayVolume, i);
        }

        //Bar.fillAmount = VolumeIndicator / maxNoiseLevel; старый статичный смособ отображения шума без помех

    }

    IEnumerator UpdateNoiseLevelEverySecond()
    {
        while (true)
        {
            UpdateNoiseLevel();
            yield return new WaitForSeconds(0.2f); 

        }
    }
    bool DisplayNoisePoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10f) >= _health);
    }

    private IEnumerator DecreaseNoiseLevelOverTime()
    {
        while (true)
        {
            DecreaseNoise(8f);
            yield return new WaitForSeconds(1f);
        }
    }
}
