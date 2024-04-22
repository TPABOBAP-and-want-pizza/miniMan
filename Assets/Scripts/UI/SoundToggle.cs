using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public Sprite imageOn;  // Иконка когда звуки включены
    public Sprite imageOff; // Иконка когда звуки выключены
    private Button button;
    private Image buttonImage;
    private bool areSoundsOn = true; // Изначальное состояние звуков

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = areSoundsOn ? imageOn : imageOff;
        }
        else
        {
            Debug.LogError("Button image component not found!");
        }
        button.onClick.AddListener(ToggleSounds);
    }

    void ToggleSounds()
    {
        areSoundsOn = !areSoundsOn;
        GlobalSoundManager soundManager = FindObjectOfType<GlobalSoundManager>();
        if (soundManager != null)
        {
            soundManager.ToggleSounds(areSoundsOn);
        }
        buttonImage.sprite = areSoundsOn ? imageOn : imageOff;
    }
}
