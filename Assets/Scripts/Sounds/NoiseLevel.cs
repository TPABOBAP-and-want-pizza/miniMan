using UnityEngine;
using UnityEngine.UI;

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
            // Здесь вызовите метод SpawnHand, передавая необходимую позицию
            HandSpawner.SpawnHandDefault(objectTransform.position); // Пример, предполагает наличие такого метода.
        }

        Bar.fillAmount = VolumeIndicator / maxNoiseLevel;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //IncreaseNoise(10f);  
            // Теперь этот вызов корректен и будет работать с любой точки игры
            //StartCoroutine(Shake(shakeDuration, shakeMagnitude));
            //DefaultShake();
            //HandSpawner.SpawnHand(transform.localPosition + new Vector3(0, 5, 11), 10);
            //IncreaseNoise(10f);
        }
    }
}
