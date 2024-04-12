using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NoiseLevel : MonoBehaviour
{
    [SerializeField] Transform objectTransform;
    private float VolumeIndicator = 0f;
    public float maxNoiseLevel = 100f;
    public Image Bar;

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
        if (VolumeIndicator >= 100f)
        {
            HandSpawner.SpawnHandDefault(objectTransform.position);
        }

        Bar.fillAmount = VolumeIndicator / maxNoiseLevel;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //StartCoroutine(Shake(shakeDuration, shakeMagnitude));
            //DefaultShake();

            //HandSpawner.SpawnHand(transform.localPosition + new Vector3(0, 5, 11), 10);
            IncreaseNoise(10f);

        }
    }
}
