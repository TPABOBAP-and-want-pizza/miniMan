using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoiseLevel : MonoBehaviour
{
    public static NoiseLevel Instance { get; private set; }

    public float CurrentNoise { get; private set; }
    public GameObject LastNoiseMaker { get; private set; }

    private float VolumeIndicator = 0f;
    public float maxNoiseLevel = 100f;
    private float Bar;
    public Image[] noisePoints;
    private GameObject LastNoiseObject;

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
        Reset();
        UpdateNoiseLevel();
        StartCoroutine(UpdateNoiseLevelEverySecond());//отображение шума
        StartCoroutine(DecreaseNoiseLevelOverTime());
    }

    public void Reset()
    {
        VolumeIndicator = 0;
        CurrentNoise = 0;
    }

    public void IncreaseNoise(float amount, GameObject noiseMaker)
    {
        VolumeIndicator += amount;
        CurrentNoise += amount;
        LastNoiseMaker = noiseMaker;
        LastNoiseObject = noiseMaker;  // Обеспечиваем, что LastNoiseObject всегда обновляется с последним объектом, создающим шум
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
            if (LastNoiseObject != null)
            {
                // Проверка, является ли LastNoiseObject игроком
                if (LastNoiseObject.CompareTag("Player"))
                {
                    HandSpawner.SpawnTrackingHand(LastNoiseObject.transform.position);
                }
                else
                {
                    HandSpawner.SpawnHandDefault(LastNoiseObject.transform.position);
                }
            }

            DecreaseNoise(50f); // Уменьшение уровня шума после реакции
        }
        // случайное колебание
        float noiseFluctuation = Random.Range(-6f, 6f);

        // показатель для Bar.fillAmount с учетом колебания
        float displayVolume = VolumeIndicator + noiseFluctuation;
        displayVolume = Mathf.Clamp(displayVolume, 0, maxNoiseLevel); // не вышло за пределы допустимых

        for (int i = 0; i < noisePoints.Length; i++)
        {
            noisePoints[i].enabled = !DisplayNoisePoint(displayVolume, i);
        }

        //Bar.fillAmount = VolumeIndicator / maxNoiseLevel; // старый статичный способ отображения шума без помех
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
