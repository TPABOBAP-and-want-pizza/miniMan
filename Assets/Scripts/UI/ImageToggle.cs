using UnityEngine;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour
{
    public Sprite image1;
    public Sprite image2;
    private Button button;
    private Image buttonImage;
    private bool isMusicOn;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = image1;
            MusicManager musicManager = FindObjectOfType<MusicManager>();
            isMusicOn = musicManager != null && musicManager.IsMusicPlaying(); // Проверяем статус музыки
        }
        else
        {
            Debug.LogError("Button image component not found!");
        }
        button.onClick.AddListener(ToggleImageAndMusic);
    }

    void ToggleImageAndMusic()
    {
        isMusicOn = !isMusicOn;
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.ToggleMusic(isMusicOn);
        }
        buttonImage.sprite = isMusicOn ? image1 : image2;
    }
}
